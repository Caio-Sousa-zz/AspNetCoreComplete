using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoRepository: IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutoByFornecedor(Guid fornecedorId);

        Task<IEnumerable<Produto>> GetProdutosFornecedores();

        Task<Produto> GetProdutoFornecedor(Guid id);
    }
}