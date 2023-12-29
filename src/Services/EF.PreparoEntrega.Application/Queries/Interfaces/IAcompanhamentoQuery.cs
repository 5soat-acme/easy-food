using EF.PreparoEntrega.Application.DTOs.Responses;

namespace EF.PreparoEntrega.Application.Queries.Interfaces;

public interface IAcompanhamentoQuery
{
    Task<IEnumerable<PedidoAcompanhamentoDto>> ObterPedidos();
}