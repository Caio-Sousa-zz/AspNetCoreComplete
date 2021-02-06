using DevIO.Business.Models;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorService
    {
        Task Add(Fornecedor fornecedor);

        Task Update(Fornecedor fornecedor);

        Task Delete(Fornecedor fornecedor);

        Task UpdateAddress(Endereco endereco);
    }
}