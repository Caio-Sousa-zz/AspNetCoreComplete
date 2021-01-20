using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository: IRepository<Fornecedor>
    {
        Task<Fornecedor> GetFornecedorEndereco(Guid id);

        Task<Fornecedor> GetFornecedorProdutosEndereco(Guid id);
    }
}