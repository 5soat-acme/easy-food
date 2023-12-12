using EF.Domain.Commons.Mediator;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.Queries;
using EF.WebApi.Commons.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EF.Api.Estoques.Controllers
{
    [Route("api/estoques")]
    public class EstoqueController : CustomControllerBase
    {
        private readonly IMediatorHandler _mediator;
        private readonly IEstoqueQuery _estoqueQuery;

        public EstoqueController(IMediatorHandler mediator, IEstoqueQuery estoqueQuery)
        {
            _mediator = mediator;
            _estoqueQuery = estoqueQuery;
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarEstoque([FromBody] AtualizarEstoqueCommand command, CancellationToken cancellationToken)
        {
            return Respond(await _mediator.Send(command, cancellationToken));
        }

        [HttpGet("{produtoId}")]
        public async Task<IActionResult> BuscarEstoque([FromRoute] Guid produtoId, CancellationToken cancellationToken)
        {
            return Respond(await _estoqueQuery.ObterEstoqueProduto(produtoId, cancellationToken));
        }
    }
}
