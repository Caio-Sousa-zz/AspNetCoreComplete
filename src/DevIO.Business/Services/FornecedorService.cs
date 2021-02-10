using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                                 IEnderecoRepository enderecoRepository,
                                 INotificador notificador): base(notificador)
        {

        }

        public async Task Add(Fornecedor fornecedor)
        {
            // Validar entidade
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) &&
                !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if (_fornecedorRepository.Get(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Ja existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Add(fornecedor);
        }

        public async Task Update(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if (_fornecedorRepository.Get(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Ja existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Update(fornecedor);
        }

        public async Task UpdateAddress(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Update(endereco);
        }

        public async Task Delete(Guid id)
        {
            if (_fornecedorRepository.GetFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return;
            }

            await _fornecedorRepository.Delete(id);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}