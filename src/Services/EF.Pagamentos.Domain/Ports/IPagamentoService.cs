using EF.Pagamentos.Domain.Models;

namespace EF.Pagamentos.Domain.Ports;

public interface IPagamentoService
{
    Task<Transacao> AutorizarPagamento(Pagamento pagamento);
}