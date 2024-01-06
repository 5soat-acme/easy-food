using EF.Domain.Commons.Repository;
using EF.Produtos.Domain.Models;
using EF.Produtos.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Produtos.Infra.Data.Repository;

public sealed class ProdutoRepository : IProdutoRepository
{
    private readonly ProdutoDbContext _dbContext;

    public ProdutoRepository(ProdutoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<Produto> Criar(Produto produto, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Produtos.AddAsync(produto);
        return result.Entity;
    }

    public Produto Atualizar(Produto produto, CancellationToken cancellationToken)
    {
        var result =  _dbContext.Produtos.Update(produto);
        return result.Entity;
    }

    public async Task<IList<Produto>> Buscar(ProdutoCategoria? categoria)
    {
       return await _dbContext.Produtos
            .Where(produto => produto.Ativo && (categoria == null || produto.Categoria == categoria))
            .ToListAsync();
    }

    public async Task<Produto?> BuscarPorId(Guid produtoId, CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos.SingleAsync(produto => produto.Id == produtoId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
