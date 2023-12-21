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
    /// <summary>
    /// Obtém o carrinho do cliente, caso esteja logado, ou o carrinho anônimo.
    /// </summary>
    /// <returns>
    ///   <see cref="CarrinhoCliente"/>
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoCliente))]
    [HttpGet]
    public async Task<IActionResult> ObterCarrinho()
    {
        return Respond(await carrinhoAppService.ObterCarrinhoCliente());
    }

    /// <summary>
    /// Adiciona um item ao carrinho. Caso o item já exista no carrinho, a quantidade é incrementada.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost]
    public async Task<IActionResult> AdicionarItem(AdicionarItemDto itemDto)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await carrinhoAppService.AdicionarItemCarrinho(itemDto);

        if (!result.IsValid)
        {
            AddErrors(result.Errors);
        }

        return Respond();
    }

    /// <summary>
    /// Atualiza a quantidade de intes no carrinho.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPut("{itemId}")]
    public async Task<IActionResult> AtualizarItem(Guid itemId, AtualizarItemDto item)
    {
        if (itemId != item.ItemId)
        {
            AddError("O item não corresponde ao informado");
            return Respond();
        }

        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await carrinhoAppService.AtualizarItem(item);

        if (!result.IsValid)
        {
            AddErrors(result.Errors);
        }

        return Respond();
    }

    /// <summary>
    /// Remove um item do carrinho.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoverItem(Guid itemId)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        await carrinhoAppService.RemoverItemCarrinho(itemId);
        return Respond();
    }

    /// <summary>
    /// Finaliza a montagem do carrinho e gera um novo pedido.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("finalizar")]
    public async Task<IActionResult> GerarPedido()
    {
        await carrinhoAppService.GerarPedido();
        return Respond();
    }
}