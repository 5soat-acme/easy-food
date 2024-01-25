using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Domain.Ports;

public interface IPagamentoService
{
    public Task<Transacao> AutorizarPagamento(Pagamento pagamento);
}