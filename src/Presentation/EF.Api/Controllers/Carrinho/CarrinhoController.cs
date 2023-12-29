using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Carrinho;

[Authorize]
[Route("api/carrinho")]
public class CarrinhoController(
    ICarrinhoConsultaService carrinhoConsultaService,
    ICarrinhoManipulacaoService carrinhoManipulacaoService,
    ICarrinhoCheckoutService carrinhoCheckoutService
) : CustomControllerBase
{
    /// <summary>
    ///     Obtém o carrinho do cliente.
    /// </summary>
    /// <remarks>
    ///     Obtém o carrinho do cliente. Caso o cliente tenha se identificado no sistema, é verificado se o mesmo possui um
    ///     carrinho em aberto. Para clientes não identificados, é criado um carrinho temporário associado ao token gerado para
    ///     o usuário anônimo.
    /// </remarks>
    /// <response code="200">Retorna dados do carrinho.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoClienteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> ObterCarrinho()
    {
        return Respond(await carrinhoConsultaService.ObterCarrinhoCliente());
    }

    /// <summary>
    ///     Adiciona um item ao carrinho.
    /// </summary>
    /// <remarks>
    ///     Adiciona um item ao carrinho. Caso o item já exista no carrinho, a quantidade é incrementada.
    /// </remarks>
    /// <response code="204">Indica que o item foi adicionado no carrinho com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> AdicionarItem(AdicionarItemDto itemDto)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await carrinhoManipulacaoService.AdicionarItemCarrinho(itemDto);

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
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CheckoutRespostaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpPost("checkout")]
    public async Task<IActionResult> IniciarCheckout()
    {
        var result = await carrinhoCheckoutService.IniciarCheckout();

        if (!result.IsValid)
        {
            AddErrors(result.Errors);
            return Respond();
        }

        return Respond(result.Data);
    }

    /// <summary>
    ///     Aplicar cupom de desconto
    /// </summary>
    /// <param name="codigo">
    ///     Código do cupom de desconto
    /// </param>
    /// <response code="204">Indica que cupom foi aplicado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpPost("cupom/{codigo}")]
    public async Task<IActionResult> AplicarCupom(string codigo)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await carrinhoManipulacaoService.AplicarCupom(codigo);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond();
    }

    /// <summary>
    ///     Atualiza a quantidade de um item no carrinho.
    /// </summary>
    /// <response code="204">Indica que a quantidade do item foi atualizada com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        var result = await carrinhoManipulacaoService.AtualizarItem(item);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond();
    }

    /// <summary>
    ///     Remove um item do carrinho.
    /// </summary>
    /// <response code="204">Indica que o item foi removido com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoverItem(Guid itemId)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        await carrinhoManipulacaoService.RemoverItemCarrinho(itemId);
        return Respond();
    }
}