using EF.Pedidos.Application.DTOs.Integrations;

namespace EF.Pedidos.Application.Ports;

public interface IProdutoService
{
    Task<ProdutoDto?> ObterProdutoPorId(Guid produtoId);
}