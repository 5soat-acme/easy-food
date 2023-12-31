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
    ///     Obtém um pedido através do Id ou Id de correlação (informado no fechamento do carrinho).
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <returns>
    ///     <see cref="PedidoDto" />
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
    [HttpGet("{pedidoId}")]
    public async Task<IActionResult> ObterPedido([FromRoute] Guid pedidoId)
    {
        var pedido = await pedidoQuery.ObterPedidoPorId(pedidoId);
        return pedido is not null ? Respond(pedido) : NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
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