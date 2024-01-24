using EF.Pagamentos.Domain.Models;
using EF.Pagamentos.Domain.Ports;

namespace EF.Pagamentos.Infra.Services;

public class PagamentoPixService : IPagamentoService
{
    public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
    {
        return new Transacao(pagamento.Id);
    }
}