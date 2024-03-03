using EF.Core.Commons.Repository;
using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Domain.Repository;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    Task<Pagamento?> ObterPorId(Guid id);
    Task<Pagamento?> ObterPorPedidoId(Guid pedidoId);
    void Criar(Pagamento pagamento);
    void Atualizar(Pagamento pagamento);
    void AdicionarTransacao(Transacao transacao);
}