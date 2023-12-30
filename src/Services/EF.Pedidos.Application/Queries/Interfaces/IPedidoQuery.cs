using EF.Pedidos.Application.DTOs.Responses;

namespace EF.Pedidos.Application.Queries.Interfaces;

public interface IPedidoQuery
{
    Task<PedidoDto?> ObterPedidoPorId(Guid pedidoId);
}