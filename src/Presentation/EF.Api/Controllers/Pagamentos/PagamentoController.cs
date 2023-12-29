using EF.Pagamentos.Application.Queries.Interfaces;
using EF.Pagamentos.Domain.Models;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Controllers.Pagamentos;

[Route("api/pagamentos")]
public class PagamentoController : CustomControllerBase
{
    private readonly IFormaPagamentoQuery _formaPagamentoQuery;

    public PagamentoController(IFormaPagamentoQuery formaPagamentoQuery)
    {
        _formaPagamentoQuery = formaPagamentoQuery;
    }

    /// <summary>
    ///     Obtém as formas de pagamento
    /// </summary>
    /// <response code="200">Retorna as formas de pagamento.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<FormaPagamento>))]
    [Produces("application/json")]
    [HttpGet("formas-pagamento")]
    public async Task<IActionResult> BuscarFormasPagamento(CancellationToken cancellationToken)
    {
        return Respond(await _formaPagamentoQuery.ObterFormasPagamento(cancellationToken));
    }
}