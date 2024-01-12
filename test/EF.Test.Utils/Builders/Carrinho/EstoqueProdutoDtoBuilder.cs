using Bogus;
using EF.Carrinho.Application.DTOs.Integrations;

namespace EF.Test.Utils.Builders.Carrinho;

public class EstoqueProdutoDtoBuilder : Faker<EstoqueProdutoDto>
{
    public EstoqueProdutoDtoBuilder()
    {
        CustomInstantiator(f =>
            new EstoqueProdutoDto
            {
                ProdutoId = Guid.NewGuid(),
                Quantidade = int.MaxValue
            });
    }

    public EstoqueProdutoDtoBuilder ProdutoId(Guid id)
    {
        RuleFor(a => a.ProdutoId, () => id);
        return this;
    }

    public EstoqueProdutoDtoBuilder Quantidade(int quantidade)
    {
        RuleFor(a => a.Quantidade, () => quantidade);
        return this;
    }
}