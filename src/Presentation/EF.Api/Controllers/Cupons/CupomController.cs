using EF.Cupons.Application.Commands;
using EF.Cupons.Application.DTOs.Responses;
using EF.Cupons.Application.DTOs.Requests;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Cupons;

[Route("api/cupons")]
public class CupomController : CustomControllerBase
{
    private readonly ICupomQuery _cupomQuery;
    private readonly IMediatorHandler _mediator;

    public CupomController(IMediatorHandler mediator,
        ICupomQuery cupomQuery)
    {
        _mediator = mediator;
        _cupomQuery = cupomQuery;
    }

    /// <summary>
    ///     Busca cupom de desconto através do código do cupom
    /// </summary>
    /// <remarks>
    ///     Será retornada as informações do cupom caso ele esteja vigente.
    /// </remarks>
    /// <response code="200">Retorna as informações do cupom.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CupomProdutoDto))]
    [Produces("application/json")]
    [HttpGet("{codigoCupom}")]
    public async Task<IActionResult> BuscarCupomProduto(string codigoCupom, CancellationToken cancellationToken)
    {
        return Respond(await _cupomQuery.ObterCupom(codigoCupom, cancellationToken));
    }

    /// <summary>
    ///     Cria cupom de desconto.
    /// </summary>
    /// <response code="200">Indica que o cupom de desconto foi criado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> CriarCupom([FromBody] CriarCupomDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CriarCupomCommand
        {
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            CodigoCupom = dto.CodigoCupom,
            PorcentagemDesconto = dto.PorcentagemDesconto,
            Produtos = dto.Produtos
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Atualiza cupom de desconto.
    /// </summary>
    /// <response code="200">Indica que o cupom de desconto foi atualizado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPut("{cupomId}")]
    public async Task<IActionResult> AtualizarCupom(Guid cupomId, [FromBody] AtualizarCupomDto dto,
        CancellationToken cancellationToken)
    {
        var command = new AtualizarCupomCommand
        {
            CupomId = cupomId,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            CodigoCupom = dto.CodigoCupom,
            PorcentagemDesconto = dto.PorcentagemDesconto
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Inativa cupom de desconto.
    /// </summary>
    /// <response code="200">Indica que o cupom de desconto foi inativado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPut("inativar/{cupomId}")]
    public async Task<IActionResult> InativarCupom(Guid cupomId, CancellationToken cancellationToken)
    {
        var command = new InativarCupomCommand
        {
            CupomId = cupomId
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Remove produtos do cupom de desconto.
    /// </summary>
    /// <response code="200">Indica que os produtos foram removidos do cupom de desconto.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpDelete("{cupomId}/remover-produtos")]
    public async Task<IActionResult> RemoverCupomProduto(Guid cupomId, [FromBody] IList<AdicionarRemoverCupomProdutoDto> produtos,
        CancellationToken cancellationToken)
    {
        var command = new RemoverProdutosCommand
        {
            CupomId = cupomId,
            Produtos = produtos.Select(x => x.ProdutoId).ToList()
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Adiciona produtos no cupom de desconto.
    /// </summary>
    /// <response code="200">Indica que os produtos foram adicionados no cupom de desconto.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPut("{cupomId}/inserir-produtos")]
    public async Task<IActionResult> InserirCupomProduto(Guid cupomId, [FromBody] IList<AdicionarRemoverCupomProdutoDto> produtos,
        CancellationToken cancellationToken)
    {
        var command = new InserirProdutosCommand
        {
            CupomId = cupomId,
            Produtos = produtos.Select(x => x.ProdutoId).ToList()
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }
}