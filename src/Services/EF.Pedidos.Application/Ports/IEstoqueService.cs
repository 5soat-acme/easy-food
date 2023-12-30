using EF.Pedidos.Application.DTOs.Integrations;

namespace EF.Pedidos.Application.Ports;

public interface IEstoqueService
{
    Task<EstoqueProdutoDto?> ObterEstoquePorProdutoId(Guid produtoId);
}