using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.DTOs;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Pedidos;

[Authorize]
[Route("api/pedidos")]
public class PedidoController(IMediatorHandler mediator, IPedidoQuery pedidoQuery) : CustomControllerBase
{
    /// <summary>
    ///     Obtém um pedido através do Id ou Id de correlação (informado no fechamento do carrinho).
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <param name="correlacaoId">Id de correlação (iformado no fechamento do carrinho)</param>
    /// <returns>
    ///     <see cref="PedidoDto" />
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PedidoDto))]
    [HttpGet]
    public async Task<IActionResult> CriarPedido([FromQuery] Guid pedidoId, [FromQuery] Guid correlacaoId)
    {
        if (pedidoId == Guid.Empty && correlacaoId == Guid.Empty)
        {
            AddError("Informe o pedidoId ou correlacaoId");
            return Respond();
        }

        if (correlacaoId != Guid.Empty) return Respond(await pedidoQuery.ObterPedidoPorCorrelacaoId(correlacaoId));

        return Respond(await pedidoQuery.ObterPedidoPorId(pedidoId));
    }
}