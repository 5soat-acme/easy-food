using EF.Domain.Commons.Repository;
using EF.PreparoEntrega.Domain.Models;
using EF.PreparoEntrega.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.PreparoEntrega.Infra.Data.Repository;

public sealed class PedidoRepository : IPedidoRepository
{
    private readonly PreparacaoEntregaDbContext _context;

    public PedidoRepository(PreparacaoEntregaDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<Pedido> ObterPedidoPorId(Guid id)
    {
        return await _context.Pedidos
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosEmAberto()
    {
        return await _context.Pedidos
            .Include(c => c.Itens)
            .Where(c => c.Status != Status.Finalizado)
            .ToListAsync();
    }

    public void Criar(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
    }

    public void Atualizar(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
    }

    public void Remover(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);
    }
}