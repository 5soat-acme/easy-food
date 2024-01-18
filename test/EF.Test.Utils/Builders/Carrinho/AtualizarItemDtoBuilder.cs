using Bogus;
using EF.Carrinho.Application.DTOs.Requests;

namespace EF.Test.Utils.Builders.Carrinho;

public class AtualizarItemDtoBuilder : Faker<AtualizarItemDto>
{
    public AtualizarItemDtoBuilder()
    {
        CustomInstantiator(f =>
            new AtualizarItemDto
            {
                Quantidade = f.Random.Int(1, 10)
            });
    }

    public AtualizarItemDtoBuilder ItemId(Guid id)
    {
        RuleFor(a => a.ItemId, () => id);
        return this;
    }
}