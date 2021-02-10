using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, 
                              INotificador notificador): base(notificador)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Add(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Add(produto);
        }

        public async Task Update(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Update(produto);
        }

        public async Task Delete(Guid id)
        {

            await _produtoRepository.Delete(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}