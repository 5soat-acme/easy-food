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

    [HttpGet]
    public async Task<IActionResult> ObterPedidos()
    {
        var pedidos = await _preparoEntregaQuery.ObterPedidosMonitor();
        return pedidos is null ? NotFound() : Respond(pedidos);
    }
}