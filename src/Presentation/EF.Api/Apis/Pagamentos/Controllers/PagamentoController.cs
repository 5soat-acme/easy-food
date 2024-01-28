using EF.Pagamentos.Application.DTOs.Responses;
using EF.Pagamentos.Domain.Models;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Apis.Pagamentos.Controllers;

[Route("api/pagamentos")]
public class PagamentoController : CustomControllerBase
{
    /// <summary>
    ///     Obtém os tipos de pagamento disponíveis.
    /// </summary>
    /// <response code="200">Retorna os tipos de pagamento.</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MetodoPagamentoDto))]
    [Produces("application/json")]
    [HttpGet("tipos")]
    public ActionResult<MetodoPagamentoDto> ObterTiposPagamento()
    {
        return Ok(new MetodoPagamentoDto { MetodosPagamento = Enum.GetNames(typeof(Tipo)) });
    }
}