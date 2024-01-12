using Bogus;
using EF.Estoques.Application.DTOs.Responses;

namespace EF.Test.Utils.Builders.Carrinho;

public class EstoqueProdutoDtoBuilder : Faker<EstoqueDto>
{
    public EstoqueProdutoDtoBuilder()
    {
        CustomInstantiator(f =>
            new EstoqueDto
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