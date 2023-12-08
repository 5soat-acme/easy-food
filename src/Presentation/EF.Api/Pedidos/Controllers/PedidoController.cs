using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.Commands;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Pedidos.Controllers;

[Route("api/pedidos")]
public class PedidoController : CustomControllerBase
{
    private readonly IMediatorHandler _mediator;

    public PedidoController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido(IncluirItemPedidoCommand command)
    {
        return Respond(await _mediator.Send(command));
    }
}