using EF.PreparoEntrega.Application.DTOs.Responses;

namespace EF.PreparoEntrega.Application.Queries.Interfaces;

public interface IPreparoQuery
{
    Task<IEnumerable<PedidoPreparoDto>> ObterPedidos();
}