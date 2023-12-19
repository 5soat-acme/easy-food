using EF.Carrinho.Application.DTOs;
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoCliente))]
    public async Task<IActionResult> ObterCarrinho()
    {
        var carrinho = await carrinhoAppService.ObterCarrinhoCliente();

        if (carrinho is null)
        {
            carrinho = new CarrinhoCliente(user.ObterCarrinhoId());
            carrinho.AssociarCliente(user.GetUserId());
        }

        return Respond(carrinho);
    }
    
    [HttpPost]
    public async Task<IActionResult> AdicionarItem(AdicionarItemDto itemDto)
    {
        if (!ModelState.IsValid) return Respond(ModelState);
        
        await carrinhoAppService.AdicionarItemCarrinho(itemDto);
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
    
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoverItem(Guid itemId, Item item)
    {
        if(itemId != item.Id)
        {
            AddError("O item não corresponde ao informado");
            return Respond();
        }
        
        await carrinhoAppService.RemoverItemCarrinho(item);
        return Respond();
    }
}