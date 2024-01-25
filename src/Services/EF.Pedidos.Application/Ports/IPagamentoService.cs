using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Application.Ports;

public interface IPagamentoService
{
    Task<bool> ProcessarPagamento(PagamentoDto pagamento);
}