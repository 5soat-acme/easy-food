using EF.Pagamentos.Application.DTOs.Requests;
using EF.Pagamentos.Application.UseCases.Interfaces;
using EF.Pedidos.Application.DTOs.Gateways;
using EF.Pedidos.Application.Gateways;

namespace EF.Pedidos.Infra.Adapters.Pagamentos;

public class PagamentoAdapter : IPagamentoService
{
    private readonly IAutorizarPagamentoUseCase _autorizarPagamento;

    public PagamentoAdapter(IAutorizarPagamentoUseCase autorizarPagamento)
    {
        _autorizarPagamento = autorizarPagamento;
    }

    public async Task<bool> ProcessarPagamento(PagamentoDto pagamento)
    {
        var result = await _autorizarPagamento.Handle(new AutorizarPagamentoDto
        {
            PedidoId = pagamento.PedidoId,
            TipoPagamento = pagamento.TipoPagamento,
            Valor = pagamento.Valor,
            Cpf = pagamento.Cpf
        });

        return result.IsValid;
    }
}