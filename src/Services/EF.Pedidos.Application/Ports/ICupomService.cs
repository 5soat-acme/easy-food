using EF.Pedidos.Application.DTOs.Integrations;

namespace EF.Pedidos.Application.Ports;

public interface ICupomService
{
    Task<CupomDescontoDto?> ObterCupomDesconto(string codigo);
}