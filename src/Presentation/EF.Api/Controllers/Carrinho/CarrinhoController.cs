using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;
using EF.WebApi.Commons.Controllers;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Carrinho;

[Authorize]
[Route("api/carrinho")]
public class CarrinhoController(ICarrinhoAppService carrinhoAppService, IUserApp user) : CustomControllerBase
{
    private readonly Guid _carrinhoId = user.ObterCarrinhoId();
    
    [HttpGet]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await carrinhoAppService.ObterCarrinhoCliente() ?? new CarrinhoCliente(_carrinhoId);
    }
    
    [HttpPost]
    public async Task<IActionResult> AdicionarItem(Item item)
    {
        await carrinhoAppService.AdicionarItemCarrinho(item);
        return Respond();
    }
    
    [HttpPut("{itemId}")]
    public async Task<IActionResult> AtualizarItem(Guid itemId, Item item)
    {
        if(itemId != item.Id)
        {
            AddError("O item não corresponde ao informado");
            return Respond();
        }
        
        await carrinhoAppService.AtualizarItem(item);
        return Respond();
    }
}