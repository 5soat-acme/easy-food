using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Services.Integrations.Interfaces;

public interface IProdutoService
{
    Task<Item> ObterItemPorProdutoId(Guid produtoId);
}