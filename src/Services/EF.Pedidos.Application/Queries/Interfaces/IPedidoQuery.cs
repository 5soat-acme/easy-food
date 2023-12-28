using EF.Pedidos.Application.DTOs;

namespace EF.Pedidos.Application.Queries.Interfaces;

public interface IPedidoQuery
{
    Task<PedidoDto?> ObterPedidoPorId(Guid pedidoId);
    Task<PedidoDto?> ObterPedidoPorCorrelacaoId(Guid correlacaoId);
}