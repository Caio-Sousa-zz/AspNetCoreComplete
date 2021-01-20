using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DevIO.Data.Context;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MeuDbContext db): base(db)
        {

        }
        public async Task<IEnumerable<Produto>> GetProdutoByFornecedor(Guid fornecedorId)
        {
            return await Get(a => a.FornecedorId == fornecedorId);
        }

        public async Task<Produto> GetProdutoFornecedor(Guid id)
        {
            return await Db.Produtos.AsNoTracking()
                                    .Include(f => f.Fornecedor) // Join with forneceder
                                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosFornecedores()
        {
            return await Db.Produtos.AsNoTracking()
                                    .Include(f => f.Fornecedor)
                                    .OrderBy(p => p.Nome)
                                    .ToListAsync();
        }
    }
}