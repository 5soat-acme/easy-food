using EF.Carrinho.Application.DTOs.Integrations;
using EF.Carrinho.Application.Services.Integrations.Interfaces;
using EF.Carrinho.Application.Services.Interfaces;

namespace EF.Carrinho.Application.Services.Integrations;

public class EstoqueService : IEstoqueService
{
    public async Task<EstoqueDto> ObterEstoquePorProdutoId(Guid produtoId)
    {
        throw new NotImplementedException();
    }
}