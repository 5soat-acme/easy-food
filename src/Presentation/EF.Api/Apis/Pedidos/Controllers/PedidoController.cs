using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.Commands.CriarPedido;
using EF.Pedidos.Application.Commands.ProcessarPagamento;
using EF.Pedidos.Application.DTOs.Responses;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Pedidos.Controllers;

[Route("api/pedidos")]
public class PedidoController(IMediatorHandler mediator, IPedidoQuery pedidoQuery, IUserApp userApp)
    : CustomControllerBase
{
    /// <summary>
    ///     Obtém um pedido.
    /// </summary>
    /// <param name="id">Id do pedido</param>
    /// <response code="200">Dados do pedido.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPedido([FromRoute] Guid id)
    {
        var pedido = await pedidoQuery.ObterPedidoPorId(id);
        return pedido is not null ? Respond(pedido) : NotFound();
    }

    /// <summary>
    ///     Faz o checkout do pedido. Esse processo efetiva o pedido que fica disponível para pagamento.
    /// </summary>
    /// <response code="200">Retorna o Id do pedido.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Checkout(CriarPedidoCommand command)
    {
        command.SessionId = userApp.GetSessionId();
        command.ClienteId = userApp.GetUserId();
        command.ClienteCpf = userApp.GetUserCpf();

        var result = await mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond(new { pedidoId = result.AggregateId });
    }

    /// <summary>
    ///     Processa o pagamento e faz a confirmação do pedido.
    /// </summary>
    /// <response code="200">Pedido confirmado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [Authorize]
    [HttpPost("{pedidoId}/confirmar")]
    public async Task<IActionResult> ProcessarPagamento([FromRoute] Guid pedidoId, ProcessarPagamentoCommand command)
    {
        if (pedidoId != command.PedidoId)
        {
            AddError("O pedido não corresponde ao informado");
            return Respond();
        }

        command.SessionId = userApp.GetSessionId();
        command.ClienteId = userApp.GetUserId();
        command.ClienteCpf = userApp.GetUserCpf();

        var result = await mediator.Send(command);

        if (!result.IsValid()) return Respond(result.ValidationResult);

        return Respond(new { pedidoId = result.AggregateId });
    }
}