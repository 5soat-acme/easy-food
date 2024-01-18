using EF.Carrinho.Application.DTOs.Requests;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Carrinho.Application.Services.Interfaces;
using EF.WebApi.Commons.Controllers;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Carrinho.Controllers;

[Authorize]
[Route("api/carrinho")]
public class CarrinhoController : CustomControllerBase
{
    private readonly ICarrinhoConsultaService _carrinhoConsultaService;
    private readonly ICarrinhoManipulacaoService _carrinhoManipulacaoService;
    private readonly CarrinhoSessaoDto _carrinhoSessao;

    public CarrinhoController(ICarrinhoConsultaService carrinhoConsultaService,
        ICarrinhoManipulacaoService carrinhoManipulacaoService,
        IUserApp userApp)
    {
        _carrinhoConsultaService = carrinhoConsultaService;
        _carrinhoManipulacaoService = carrinhoManipulacaoService;
        _carrinhoSessao = new CarrinhoSessaoDto
        {
            CarrinhoId = userApp.GetSessionId(),
            ClienteId = userApp.GetUserId()
        };
    }

    /// <summary>
    ///     Obtém o carrinho do cliente.
    /// </summary>
    /// <remarks>
    ///     Obtém o carrinho do cliente. Caso o cliente tenha se identificado no sistema, é verificado se o mesmo possui um
    ///     carrinho em aberto. Para clientes não identificados, é criado um carrinho temporário associado ao token gerado para
    ///     o usuário anônimo.
    ///     Esse endpoint é utilizado para exibir os dados na tela de carrinho e o resumo do pedido antes da confirmação.
    /// </remarks>
    /// <response code="200">Dados do carrinho.</response>
    /// <response code="401">Não autorizado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CarrinhoClienteDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> ObterCarrinho()
    {
        return Respond(await _carrinhoConsultaService.ConsultarCarrinho(_carrinhoSessao));
    }

    /// <summary>
    ///     Adiciona um item ao carrinho.
    /// </summary>
    /// <remarks>
    ///     Adiciona um item ao carrinho. Caso o item já exista, a quantidade é incrementada.
    /// </remarks>
    /// <response code="204">Item adicionado com sucesso</response>
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

        var result = await _carrinhoManipulacaoService.AdicionarItemCarrinho(itemDto, _carrinhoSessao);

        if (!result.IsValid) AddErrors(result.Errors);

        return Respond();
    }

    /// <summary>
    ///     Atualiza a quantidade de um item no carrinho.
    /// </summary>
    /// <response code="204">Carrinho atualizado com sucesso</response>
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

        var result = await _carrinhoManipulacaoService.AtualizarItem(item, _carrinhoSessao);

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

        await _carrinhoManipulacaoService.RemoverItemCarrinho(itemId, _carrinhoSessao);
        return Respond();
    }
}