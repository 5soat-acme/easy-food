using EF.Carrinho.Application.Services.Integrations.Interfaces;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Services.Integrations;

public class ProdutoService : IProdutoService
{
    public async Task<Item> ObterItemPorProdutoId(Guid produtoId)
    {
        throw new NotImplementedException();
    }
}