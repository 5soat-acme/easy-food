using EF.Domain.Commons.Mediator;
using EF.Pedidos.Application.Commands;
using EF.WebApi.Commons.Controllers;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Pedidos;

[Authorize]
[Route("api/pedidos")]
public class PedidoController(IMediatorHandler mediator, IUserApp user) : CustomControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarPedido(IncluirItemPedidoCommand command)
    {
        command.CarrinhoId = user.ObterCarrinhoId();
        return Respond(await mediator.Send(command));
    }
}