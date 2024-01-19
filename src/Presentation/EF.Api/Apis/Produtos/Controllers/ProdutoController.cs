using EF.Domain.Commons.Mediator;
using EF.Produtos.Application.Commands;
using EF.Produtos.Application.DTOs.Responses;
using EF.Produtos.Application.Queries.Interfaces;
using EF.Produtos.Domain.Models;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Produtos.Controllers;

[Route("api/produto")]
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
    public async Task<IActionResult> Obter(ProdutoCategoria categoria)
    {
        var pedidos = await _produtoQuery.Buscar(categoria);
        return pedidos is null ? NotFound() : Respond(pedidos);
    }

    /// <summary>
    ///     Cadastra um produto
    /// </summary>
    /// <response code="200">Produto cadastrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoCommand produto)
    {
        var result = await _mediator.Send(produto);
        return Respond(result);
    }
    
    /// <summary>
    ///     Atualiza um produto
    /// </summary>
    /// <response code="200">Produto cadastrado.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarProdutoCommand produto)
    {
        if (id != produto.ProdutoId)
        {
            AddError("O produto não corresponde ao informado");
            return Respond();
        }
        
        if (!ModelState.IsValid) return Respond(ModelState);
        
        var result = await _mediator.Send(produto);
        return Respond(result);
    }
    
    /// <summary>
    ///     Remove um produto
    /// </summary>
    /// <response code="200">Produto removido.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
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