using EF.Domain.Commons.Mediator;
using EF.PreparoEntrega.Application.Commands.ConfirmarEntrega;
using EF.PreparoEntrega.Application.Commands.FinalizarPreparo;
using EF.PreparoEntrega.Application.Commands.IniciarPreparo;
using EF.PreparoEntrega.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.PreparoEntrega.Controllers;

[Route("api/preparo")]
public class PreparoController : CustomControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly IPreparoEntregaQuery _preparoEntregaQuery;

    public PreparoController(IMediatorHandler mediator, IPreparoEntregaQuery preparoEntregaQuery)
    {
        _mediator = mediator;
        _preparoEntregaQuery = preparoEntregaQuery;
    }

    [HttpGet]
    public async Task<IActionResult> ObterPedidos()
    {
        var pedidos = await _preparoEntregaQuery.ObterPedidos();
        return pedidos is null ? NotFound() : Respond(pedidos);
    }

    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarPreparo(IniciarPreparoCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }

    [HttpPost("finalizar")]
    public async Task<IActionResult> FinalizarPreparo(FinalizarPreparoCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }

    [HttpPost("confirmar-entrega")]
    public async Task<IActionResult> ConfirmarEntrega(ConfirmarEntregaCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }
}