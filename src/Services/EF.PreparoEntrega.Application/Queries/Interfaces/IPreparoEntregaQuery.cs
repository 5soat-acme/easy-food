using EF.PreparoEntrega.Application.DTOs.Responses;

namespace EF.PreparoEntrega.Application.Queries.Interfaces;

public interface IPreparoEntregaQuery
{
    Task<IEnumerable<PedidoPreparoDto>?> ObterPedidos();
    Task<IEnumerable<PedidoMonitorDto>?> ObterPedidosMonitor();
}