using Bogus;
using EF.Carrinho.Domain.Models;

namespace EF.Carrinho.Domain.Test.Fixtures;

[CollectionDefinition(nameof(CarrinhoClienteCollection))]
public class CarrinhoClienteCollection : ICollectionFixture<CarrinhoClienteFixture>
{
}

public class CarrinhoClienteFixture : IDisposable
{
    public void Dispose()
    {
    }

    public CarrinhoCliente ObterCarrinhoNovo()
    {
        return new CarrinhoCliente(Guid.NewGuid());
    }

    public Item GerarItemValido()
    {
        return GerarItensValidos(1).FirstOrDefault()!;
    }

    public List<Item> GerarItensValidos(int quantidade)
    {
        return new Faker<Item>("pt_BR")
            .CustomInstantiator(f =>
                new Item(f.Random.Guid(), f.Commerce.ProductName(), f.Random.Decimal(20, 50), f.Random.Int(1, 5)))
            .Generate(quantidade);
    }
}