using EF.Domain.Commons.Repository;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;

namespace EF.Pedidos.Infra.Data.Repository;

public sealed class PedidoRepository : IPedidoRepository
{
    private readonly PedidoDbContext _dbContext;

    public PedidoRepository(PedidoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task<Pedido> Criar(Pedido pedido)
    {
        var result = await _dbContext.Pedidos.AddAsync(pedido);
        return result.Entity;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}