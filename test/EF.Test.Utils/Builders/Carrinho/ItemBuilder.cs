using Bogus;
using EF.Carrinho.Domain.Models;

namespace EF.Test.Utils.Builders.Carrinho;

public class ItemBuilder : Faker<Item>
{
    public ItemBuilder()
    {
        CustomInstantiator(f =>
            new Item(f.Random.Guid(), f.Commerce.ProductName(), f.Random.Decimal(20, 50), f.Random.Int(1, 20)));
    }

    public ItemBuilder ProdutoId(Guid id)
    {
        RuleFor(a => a.ProdutoId, () => id);
        return this;
    }
}