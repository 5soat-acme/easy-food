using Bogus;
using EF.Carrinho.Application.DTOs.Requests;

namespace EF.Commons.Test.Builders.Carrinho.Dtos;

public sealed class AdicionarItemDtoBuilder : Faker<AdicionarItemDto>
{
    public AdicionarItemDtoBuilder()
    {
        RuleFor(m => m.Quantidade, f => f.Random.Int(1, 3));
        RuleFor(m => m.ProdutoId, f => f.Random.Guid());
    }

    public AdicionarItemDtoBuilder WithQuantidade(int quantidade)
    {
        RuleFor(m => m.Quantidade, () => quantidade);
        return this;
    }

    public AdicionarItemDtoBuilder WithProdutoId(Guid produtoId)
    {
        RuleFor(m => m.ProdutoId, () => produtoId);
        return this;
    }
}