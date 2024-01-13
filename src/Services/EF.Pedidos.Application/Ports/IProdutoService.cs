using EF.Pedidos.Application.DTOs.Adapters;

namespace EF.Pedidos.Application.Ports;

public interface IProdutoService
{
    Task<ProdutoDto> ObterPorId(Guid id);
}