using EF.PreparoEntrega.Application.DTOs.Responses;
using EF.PreparoEntrega.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.PreparoEntrega.Controllers;

[Route("api/monitor")]
public class MonitorController : CustomControllerBase
{
    private readonly IPreparoEntregaQuery _preparoEntregaQuery;

    public MonitorController(IPreparoEntregaQuery preparoEntregaQuery)
    {
        _preparoEntregaQuery = preparoEntregaQuery;
    }

    /// <summary>
    /// Obtém os dados de acompanhamento utilizados para exibir o status do pedido na tela de acompanhamento.
    /// </summary>
    /// <response code="200">Status dos pedidos.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PedidoMonitorDto>))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> ObterPedidos()
    {
        var pedidos = await _preparoEntregaQuery.ObterPedidosMonitor();
        return pedidos is null ? NotFound() : Respond(pedidos);
    }
}