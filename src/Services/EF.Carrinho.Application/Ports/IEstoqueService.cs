using EF.Carrinho.Application.DTOs.Integrations;

namespace EF.Carrinho.Application.Ports;

public interface IEstoqueService
{
    Task<ProdutoEstoqueDto?> ObterEstoquePorProdutoId(Guid produtoId);
}