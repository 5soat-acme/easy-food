using EF.Cupons.Application.Commands;
using EF.Cupons.Application.DTOs;
using EF.Cupons.Application.Queries;
using EF.Domain.Commons.Mediator;
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

    [HttpGet("{codigoCupom}")]
    public async Task<IActionResult> InserirCupomProduto(string codigoCupom, CancellationToken cancellationToken)
    {
        return Respond(await _cupomQuery.ObterCupom(codigoCupom, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CriarCupom([FromBody] CriarCupomCommand command,
        CancellationToken cancellationToken)
    {
        return Respond(await _mediator.Send(command, cancellationToken));
    }

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

    [HttpPut("inativar/{cupomId}")]
    public async Task<IActionResult> InativarCupom(Guid cupomId, CancellationToken cancellationToken)
    {
        var command = new InativarCupomCommand
        {
            CupomId = cupomId
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{cupomId}/remover-produtos")]
    public async Task<IActionResult> RemvoverCupomProduto(Guid cupomId, [FromBody] IList<CupomProdutoDto> produtos,
        CancellationToken cancellationToken)
    {
        var command = new RemoverProdutosCommand
        {
            CupomId = cupomId,
            Produtos = produtos.Select(x => x.ProdutoId).ToList()
        };

        return Respond(await _mediator.Send(command, cancellationToken));
    }

    [HttpPut("{cupomId}/inserir-produtos")]
    public async Task<IActionResult> InserirCupomProduto(Guid cupomId, [FromBody] IList<CupomProdutoDto> produtos,
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