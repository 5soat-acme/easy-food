using Bogus;
using EF.Carrinho.Application.DTOs.Requests;

namespace EF.Test.Utils.Builders.Carrinho;

public class AdicionarItemDtoBuilder : Faker<AdicionarItemDto>
{
    public AdicionarItemDtoBuilder()
    {
        CustomInstantiator(f =>
            new AdicionarItemDto
            {
                Quantidade = f.Random.Int(1, 10),
                ProdutoId = Guid.NewGuid()
            });
    }
}