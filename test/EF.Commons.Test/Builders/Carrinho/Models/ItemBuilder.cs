using Bogus;
using EF.Carrinho.Domain.Models;

namespace EF.Commons.Test.Builders.Carrinho.Models;

public sealed class ItemBuilder : Faker<Item>
{
    public ItemBuilder()
    {
        RuleFor(m => m.Id, f => f.Random.Guid());
        RuleFor(m => m.ProdutoId, f => f.Random.Guid());
        RuleFor(m => m.Quantidade, f => f.Random.Int(1, 3));
        RuleFor(m => m.Nome, f => f.Commerce.ProductName());
        RuleFor(m => m.ValorUnitario, f => f.Random.Decimal(1, 100));
        RuleFor(m => m.TempoEstimadoPreparo, f => f.Random.Int(1, 30));
    }

    public ItemBuilder WithProdutoId(Guid produtoId)
    {
        RuleFor(m => m.ProdutoId, () => produtoId);
        return this;
    }

    public ItemBuilder WithQuantidade(int quantidade)
    {
        RuleFor(m => m.Quantidade, () => quantidade);
        return this;
    }
}