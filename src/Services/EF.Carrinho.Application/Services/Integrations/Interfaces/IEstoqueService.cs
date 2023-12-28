using EF.Carrinho.Application.DTOs.Integrations;

namespace EF.Carrinho.Application.Services.Integrations.Interfaces;

public interface IEstoqueService
{
    Task<EstoqueDto> ObterEstoquePorProdutoId(Guid produtoId);
}