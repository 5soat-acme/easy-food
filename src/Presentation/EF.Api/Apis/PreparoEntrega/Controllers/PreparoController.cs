using EF.Domain.Commons.Mediator;
using EF.PreparoEntrega.Application.Commands.ConfirmarEntrega;
using EF.PreparoEntrega.Application.Commands.FinalizarPreparo;
using EF.PreparoEntrega.Application.Commands.IniciarPreparo;
using EF.PreparoEntrega.Application.DTOs.Responses;
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

    /// <summary>
    ///     Obtém os dados de pedidos para serem preparados.
    /// </summary>
    /// <response code="200">Pedidos a serem preparados.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PedidoMonitorDto>))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> ObterPedidosPreparo()
    {
        var pedidos = await _preparoEntregaQuery.ObterPedidos();
        return pedidos is null ? NotFound() : Respond(pedidos);
    }

    /// <summary>
    ///     Sinaliza o início do preparo de um pedido (Status = Em Preparacao).
    /// </summary>
    /// <response code="200">Status do pedido alterado com sucesso.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarPreparo(IniciarPreparoCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }

    /// <summary>
    ///     Sinaliza que o pedido está pronto para ser entregue (Status = Pronto)
    /// </summary>
    /// <response code="200">Status do pedido alterado com sucesso.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpPost("finalizar")]
    public async Task<IActionResult> FinalizarPreparo(FinalizarPreparoCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }

    /// <summary>
    ///     Sinaliza que o pedido está pronto para ser entregue (Status = Finalizado)
    /// </summary>
    /// <response code="200">Status do pedido alterado com sucesso.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpPost("confirmar-entrega")]
    public async Task<IActionResult> ConfirmarEntrega(ConfirmarEntregaCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond();
    }
}