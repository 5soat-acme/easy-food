using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.Commands.CriarPedido;
using EF.Pedidos.Application.DTOs.Responses;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Pedidos.Controllers;

[Authorize]
[Route("api/pedidos")]
public class PedidoController(IMediatorHandler mediator, IPedidoQuery pedidoQuery, IUserApp userApp)
    : CustomControllerBase
{
    /// <summary>
    ///     Obtém um pedido.
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="200">Dados do pedido.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpGet("{pedidoId}")]
    public async Task<IActionResult> ObterPedido([FromRoute] Guid pedidoId)
    {
        var pedido = await pedidoQuery.ObterPedidoPorId(pedidoId);
        return pedido is not null ? Respond(pedido) : NotFound();
    }

    /// <summary>
    ///     Faz o checkout do pedido.
    /// </summary>
    /// <response code="200">Checkout realizado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Checkout(CriarPedidoCommand command)
    {
        command.SessionId = userApp.GetSessionId();
        command.ClienteId = userApp.GetUserId();

        var result = await mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond(result.AggregateId);
    }
}