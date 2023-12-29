using EF.Pagamentos.Application.DTOs.Responses;

namespace EF.Pagamentos.Application.Queries.Interfaces;

public interface IFormaPagamentoQuery
{
    Task<IList<FormaPagamentoDto>> ObterFormasPagamento(CancellationToken cancellationToken);
}