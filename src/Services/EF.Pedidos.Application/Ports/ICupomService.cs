using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Application.Ports;

public interface ICupomService
{
    Task<CupomDto?> OpterCupomPorCodigo(string codigo);
}