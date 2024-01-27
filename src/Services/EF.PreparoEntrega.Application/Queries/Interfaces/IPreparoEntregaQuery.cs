using EF.PreparoEntrega.Application.DTOs.Responses;

namespace EF.PreparoEntrega.Application.Queries.Interfaces;

public interface IPreparoEntregaQuery
{
    Task<PedidoPreparoDto> ObterPedidoPorId(Guid id);
    Task<IEnumerable<PedidoPreparoDto>?> ObterPedidos();
    Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor();
}