using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.DTOs.Requests;
using EF.Estoques.Application.DTOs.Responses;
using EF.Estoques.Application.Queries.Interfaces;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Estoques.Controllers;

[Route("api/estoques")]
public class EstoqueController : CustomControllerBase
{
    private readonly IEstoqueQuery _estoqueQuery;
    private readonly IMediatorHandler _mediator;

    public EstoqueController(IMediatorHandler mediator, IEstoqueQuery estoqueQuery)
    {
        _mediator = mediator;
        _estoqueQuery = estoqueQuery;
    }

    /// <summary>
    ///     Atualiza o estoque do produto.
    /// </summary>
    /// <remarks>
    ///     A quantidade em estoque será incrementada ou decrementada de acordo com os valores informados.
    /// </remarks>
    /// <response code="200">Indica que o estoque foi atualizado com sucesso.</response>
    /// <response code="400">A solicitação está malformada e não pode ser processada.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> AtualizarEstoque([FromBody] AtualizarEstoqueDto dto,
        CancellationToken cancellationToken)
    {
        var command = new AtualizarEstoqueCommand
        {
            ProdutoId = dto.ProdutoId,
            Quantidade = dto.Quantidade,
            TipoMovimentacao = dto.TipoMovimentacao,
            OrigemMovimentacao = dto.OrigemMovimentacao
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    /// <summary>
    ///     Obtém a quantidade em estoque do produto
    /// </summary>
    /// <response code="200">Retorna o estoque do produto.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstoqueDto))]
    [Produces("application/json")]
    [HttpGet("{produtoId}")]
    public async Task<IActionResult> BuscarEstoque([FromRoute] Guid produtoId, CancellationToken cancellationToken)
    {
        return Respond(await _estoqueQuery.ObterEstoqueProduto(produtoId, cancellationToken));
    }
}