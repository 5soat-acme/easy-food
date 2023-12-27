using Bogus;
using EF.Estoques.Domain.Models;

namespace EF.Estoques.Domain.Test.Model;

[CollectionDefinition(nameof(EstoqueCollection))]
public class EstoqueCollection : ICollectionFixture<EstoqueFixture>
{
}

public class EstoqueFixture : IDisposable
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public Estoque GerarEstoque(int? qtdEstoque = null)
    {
        return GerarEstoques(1, qtdEstoque).FirstOrDefault()!;
    }

    public Estoque GerarEstoqueInvalido()
    {
        return new Estoque(Guid.Empty, 1);
    }

    public List<Estoque> GerarEstoques(int quantidade, int? qtdEstoque)
    {
        var estoques = new Faker<Estoque>("pt_BR")
            .CustomInstantiator(f => new Estoque(Guid.NewGuid(), qtdEstoque ?? f.Random.Number(1, 10)));

        return estoques.Generate(quantidade);
    }

    public MovimentacaoEstoque GerarMovimentacao(Guid estoqueId, TipoMovimentacaoEstoque? tipoMovimentacao = null,
        OrigemMovimentacaoEstoque? origemMovimentacao = null)
    {
        return GerarMovimentacoes(1, estoqueId, tipoMovimentacao, origemMovimentacao).FirstOrDefault()!;
    }

    public List<MovimentacaoEstoque> GerarMovimentacoes(int quantidade, Guid estoqueId,
        TipoMovimentacaoEstoque? tipoMovimentacaoParam = null,
        OrigemMovimentacaoEstoque? origemMovimentacaoParam = null)
    {
        var tipoMovimentacao = tipoMovimentacaoParam ??
                               GetRandomEnum<TipoMovimentacaoEstoque>(Enum.GetValues(typeof(TipoMovimentacaoEstoque)));

        OrigemMovimentacaoEstoque origemMovimentacao;
        if (origemMovimentacaoParam is null)
        {
            var origemEntrada = new[] { OrigemMovimentacaoEstoque.Compra, OrigemMovimentacaoEstoque.CancelamentoVenda };
            var origemSaida = new[] { OrigemMovimentacaoEstoque.Venda };

            if (tipoMovimentacao == TipoMovimentacaoEstoque.Entrada)
                origemMovimentacao = GetRandomEnum<OrigemMovimentacaoEstoque>(origemEntrada);
            else
                origemMovimentacao = GetRandomEnum<OrigemMovimentacaoEstoque>(origemSaida);
        }
        else
        {
            origemMovimentacao = (OrigemMovimentacaoEstoque)origemMovimentacaoParam;
        }

        var itens = new Faker<MovimentacaoEstoque>("pt_BR")
            .CustomInstantiator(f => new MovimentacaoEstoque(estoqueId, Guid.NewGuid(), f.Random.Number(5),
                tipoMovimentacao, origemMovimentacao!, DateTime.Now));

        return itens.Generate(quantidade);
    }

    public T GetRandomEnum<T>(Array values) where T : Enum
    {
        Random random = new();
        var enumRetorno = (T)values.GetValue(random.Next(values.Length))!;

        return enumRetorno;
    }
}