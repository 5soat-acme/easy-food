using EF.Pedidos.Application.DTOs.Gateways;

namespace EF.Pedidos.Application.Gateways;

public interface IPagamentoService
{
    Task<bool> ProcessarPagamento(PagamentoDto pagamento);
}