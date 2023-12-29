using EF.Domain.Commons.Repository;
using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Domain.Repository;

public interface IFormaPagamentoRepository : IRepository<FormaPagamento>
{
    Task<IList<FormaPagamento>> BuscarTodas(CancellationToken cancellationToken);
    Task<FormaPagamento?> BuscarPorTipo(TipoFormaPagamento tipoFormaPagamento, CancellationToken cancellationToken);
}