using EF.Domain.Commons.Repository;
using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Domain.Repository;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    Task<Pagamento> Criar(Pagamento pagamento, CancellationToken cancellationToken);
}