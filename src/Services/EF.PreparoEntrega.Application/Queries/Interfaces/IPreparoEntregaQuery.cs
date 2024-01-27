using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Domain.Models;

namespace EF.PreparoEntrega.Application.Queries.Interfaces;

public interface IPreparoEntregaQuery
{
    Task<PedidoPreparoDto> ObterPedidoPorId(Guid id);
    Task<IEnumerable<PedidoPreparoDto>?> ObterPedidos(StatusPreparo? status);
    Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor();
}