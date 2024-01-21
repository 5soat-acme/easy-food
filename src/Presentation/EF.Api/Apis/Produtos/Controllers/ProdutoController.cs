using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Produtos.Application.Commands;
using EF.Produtos.Application.DTOs.Requests;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;
using EF.Produtos.Domain.Models;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Produtos.Controllers;

[Route("api/produtos")]
public class ProdutoController : CustomControllerBase
{
    private readonly IProdutoQuery _produtoQuery;
    private readonly IMediatorHandler _mediator;

    public ProdutoController(IProdutoQuery produtoQuery, IMediatorHandler mediator)
    {
        _produtoQuery = produtoQuery;
        _mediator = mediator;
    }

    /// <summary>
    ///     Obtém o produto por categoria
    /// </summary>
    /// <response code="200">Lista de produtos por categoria.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProdutoDto>))]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> Obter([FromQuery] ProdutoCategoria categoria)
    {
        var pedidos = await _produtoQuery.Buscar(categoria);
        return pedidos is null ? NotFound() : Respond(pedidos);
    }

    /// <summary>
    ///     Cadastra um produto
    /// </summary>
    /// <response code="200">Produto cadastrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoDto produto)
    {
        var command = new CriarProdutoCommand
        {
            Nome = produto.Nome,
            ValorUnitario = produto.ValorUnitario,
            Categoria = produto.Categoria,
            TempoPreparoEstimado = produto.TempoPreparoEstimado,
            Descricao = produto.Descricao
        };

        var result = await _mediator.Send(command);
        return Respond(result);
    }

    /// <summary>
    ///     Atualiza um produto
    /// </summary>
    /// <response code="200">Produto cadastrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarProdutoDto produto)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var command = new AtualizarProdutoCommand
        {
            ProdutoId = id,
            Nome = produto.Nome,
            ValorUnitario = produto.ValorUnitario,
            Categoria = produto.Categoria,
            TempoPreparoEstimado = produto.TempoPreparoEstimado,
            Descricao = produto.Descricao,
            Ativo = produto.Ativo
        };

        var result = await _mediator.Send(command);
        return Respond(result);
    }

    /// <summary>
    ///     Remove um produto
    /// </summary>
    /// <response code="200">Produto removido.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [Produces("application/json")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        if (!ModelState.IsValid) return Respond(ModelState);

        var result = await _mediator.Send(new RemoverProdutoCommand
        {
            ProdutoId = id
        });

        return Respond(result);
    }
}