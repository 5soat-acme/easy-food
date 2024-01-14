using EF.Domain.Commons.Mediator;
using EF.Pagamentos.Application.Commands;
using EF.Pedidos.Application.DTOs.Adapters;
using EF.Pedidos.Application.Ports;

namespace EF.Pedidos.Infra.Adapters.Pagamentos;

public class PagamentoAdapter : IPagamentoService
{
    private readonly IMediatorHandler _mediator;

    public PagamentoAdapter(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> ProcessarPagamento(PagamentoDto pagamento)
    {
        var result = await _mediator.Send(new AutorizarPagamentoCommand
        {
            PedidoId = pagamento.PedidoId,
            TipoPagamento = pagamento.TipoPagamento,
            Valor = pagamento.Valor,
            Cpf = pagamento.Cpf
        });

        return result.IsValid();
    }
}