using EF.Carrinho.Application.DTOs;
using EF.Carrinho.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Carrinho;

[Authorize]
[Route("api/carrinho")]
public class CarrinhoController(ICarrinhoAppService carrinhoAppService) : CustomControllerBase
{
    /// <summary>
    ///     Obtém o carrinho do cliente.
    /// </summary>
    /// <remarks>
    ///     Obtém o carrinho do cliente. Caso o cliente tenha se identificado no sistema, é verificado se o mesmo possui um
    ///     carrinho em aberto.
    /// </remarks>
    /// <param name="resumo">
    ///     Se 'true', inclui algumas informações a mais no retorno, como Desconto, Valor Final (Valor Total - Desconto) e
    ///     Estimativa de Tempo de Preparo
    /// </param>
    /// <response code="200">Retorna dados do carrinho.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoClienteDto))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> ObterCarrinho([FromQuery] bool resumo = false)
    {
        if (resumo) return Respond(await carrinhoAppService.ObterResumo());

        return Respond(await carrinhoAppService.ObterCarrinhoCliente());
    }

    /// <summary>
    ///     Adiciona um item ao carrinho.
    /// </summary>
    /// <remarks>
    ///     Adiciona um item ao carrinho. Caso o item já exista no carrinho, a quantidade é incrementada.
    /// </remarks>
    /// <response code="204">Indica que o item foi adicionado no carrinho com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> AdicionarItem(AdicionarItemDto itemDto)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await carrinhoAppService.AdicionarItemCarrinho(itemDto);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond();
    }

    /// <summary>
    ///     Finaliza a montagem do carrinho e gera um novo pedido.
    /// </summary>
    /// <remarks>
    ///     Finaliza a montagem do carrinho e gera um novo pedido. O carrinho é esvaziado após a criação do pedido e um
    ///     identificador de correlação é retornado para que o pedido possa ser recuperado posteriormente.
    /// </remarks>
    /// <response code="200">Indica que o pedido foi criado com sucesso e retorna o ID de Correlação.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoFechadoRespostaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost("fechar-pedido")]
    public async Task<IActionResult> FecharPedido()
    {
        var result = await carrinhoAppService.FecharPedido();
        return Respond(result.Data);
    }

    /// <summary>
    ///     Atualiza a quantidade de um item no carrinho.
    /// </summary>
    /// <response code="204">Indica que a quantidade do item foi atualizada com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
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

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond();
    }

    /// <summary>
    ///     Remove um item do carrinho.
    /// </summary>
    /// <response code="204">Indica que o item foi removido com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoverItem(Guid itemId)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        await carrinhoAppService.RemoverItemCarrinho(itemId);
        return Respond();
    }
}