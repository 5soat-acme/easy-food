using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Carrinho;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/carrinho")]
public class CarrinhoController(ICarrinhoAppService carrinhoAppService, IUserApp user)
{
    private readonly ICarrinhoAppService _carrinhoAppService = carrinhoAppService;

    [HttpGet]
    public async Task<ActionResult<CarrinhoCliente>> ObterCarrinho()
    {
        if (user.GetUserId() != Guid.Empty)
        {
            return await _carrinhoAppService.ObterCarrinhoPorCliente(user.GetUserId());
        }

        return await _carrinhoAppService.ObterCarrinhoPorId(user.GetTokenIdentifier());
    }
}