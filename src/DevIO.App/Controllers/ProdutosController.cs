using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DevIO.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutosController(IProdutoRepository produtoRepository,
                                  IFornecedorRepository fornecedorRepository,
                                  IMapper mapper,
                                  IProdutoService produtoService,
                                  INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _produtoService = produtoService;
        }

        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.GetProdutosFornecedores()));
        }

        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await GetProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            ProdutoViewModel p = await PopulateFornecedores(new ProdutoViewModel());

            return View(p);
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopulateFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefix = $"{Guid.NewGuid()}_";

            if (!await UploadFile(produtoViewModel.ImagemUpload, imgPrefix))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = imgPrefix + produtoViewModel.ImagemUpload.FileName;

            await _produtoService.Add(_mapper.Map<Produto>(produtoViewModel));

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await GetProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("editar-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var productUpdate = await GetProduto(id);
            produtoViewModel.Fornecedor = productUpdate.Fornecedor;
            produtoViewModel.Imagem = productUpdate.Imagem;

            if (!ModelState.IsValid) return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefix = $"{Guid.NewGuid()}_";

                if (!await UploadFile(produtoViewModel.ImagemUpload, imgPrefix))
                {
                    return View(produtoViewModel);
                }

                productUpdate.Imagem = imgPrefix + produtoViewModel.ImagemUpload.FileName;
            }

            productUpdate.Nome = produtoViewModel.Nome;
            productUpdate.Descricao = produtoViewModel.Descricao;
            productUpdate.Valor = produtoViewModel.Valor;
            productUpdate.Ativo = produtoViewModel.Ativo;

            await _produtoService.Update(_mapper.Map<Produto>(productUpdate));

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await GetProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null) return NotFound(produto);

            await _produtoService.Delete(id);

            if (!OperacaoValida()) return View();

            TempData["Sucesso"] = $"Produto {produto.Nome} excluido com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> GetProduto(Guid id)
        {
            var p = _mapper.Map<ProdutoViewModel>(await _produtoRepository.GetProdutoFornecedor(id));

            p.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());

            return p;
        }

        private async Task<ProdutoViewModel> PopulateFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());

            return produto;
        }

        private async Task<bool> UploadFile(IFormFile file, string filePrefix)
        {
            if (file.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "Arquivo inválido!");

                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", filePrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");

                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}