using EF.Carrinho.Application.DTOs.Integrations;

namespace EF.Carrinho.Application.Ports;

public interface ICupomService
{
    Task<CupomDescontoDto?> ObterDescontoCupom(string codigo);
}