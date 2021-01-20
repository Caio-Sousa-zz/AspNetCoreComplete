using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(MeuDbContext db) : base(db)
        {}
        public async Task<Endereco> GetEnderecoByFornecedor(Guid fornecedorId)
        {
           return await Db.Enderecos.AsNoTracking()
                          .Include(f=>f.Fornecedor)
                          .Where(f => f.FornecedorId == fornecedorId)
                          .FirstOrDefaultAsync();
        }
    }
}