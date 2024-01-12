using Bogus;
using EF.Carrinho.Domain.Models;

namespace EF.Test.Utils.Builders.Carrinho;

public class CarrinhoClienteBuilder : Faker<CarrinhoCliente>
{
    public CarrinhoClienteBuilder()
    {
        CustomInstantiator(f => new CarrinhoCliente());
    }
}