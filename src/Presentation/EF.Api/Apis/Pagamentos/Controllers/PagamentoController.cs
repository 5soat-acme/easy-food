using EF.Pagamentos.Application.DTOs.Responses;
using EF.Pagamentos.Application.Queries.Interfaces;
using EF.Pagamentos.Domain.Models;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Pagamentos.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MetodoPagamentoDto))]
    [Produces("application/json")]
    [HttpGet("formas-pagamento")]
    public ActionResult<MetodoPagamentoDto> BuscarFormasPagamento()
    {
        return Ok(new MetodoPagamentoDto { MetodosPagamento = Enum.GetNames(typeof(TipoFormaPagamento)) });
    }
}