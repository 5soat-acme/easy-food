using EF.Domain.Commons.Repository;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Domain.Repository;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido> Criar(Pedido pedido);
}