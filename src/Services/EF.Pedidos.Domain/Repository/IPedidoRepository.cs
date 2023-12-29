using EF.Domain.Commons.Repository;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Domain.Repository;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido> ObterPorId(Guid id);
    Task<Pedido> ObterPorCorrelacaoId(Guid correlacaoId);
    void Criar(Pedido pedido);
    void Atualizar(Pedido pedido);
}