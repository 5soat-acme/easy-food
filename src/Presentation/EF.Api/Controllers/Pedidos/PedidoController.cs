using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.Commands;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Pedidos;

[Route("api/pedidos")]
public class PedidoController(IMediatorHandler mediator) : CustomControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarPedido(IncluirItemPedidoCommand command)
    {
        return Respond(await mediator.Send(command));
    }
}