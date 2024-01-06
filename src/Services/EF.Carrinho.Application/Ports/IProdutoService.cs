using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Application.Ports;

public interface IProdutoService
{
    Task<Item> ObterItemPorProdutoId(Guid produtoId);
}