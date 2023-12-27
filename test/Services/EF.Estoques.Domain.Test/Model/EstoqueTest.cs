using EF.Domain.Commons.DomainObjects;
using EF.Estoques.Domain.Models;
using FluentAssertions;

namespace EF.Estoques.Domain.Test.Model;

[Collection(nameof(EstoqueCollection))]
public class EstoqueTest(EstoqueFixture fixture)
{
    [Fact(DisplayName = "Novo estoque válido")]
    [Trait("Category", "Domain - Estoque")]
    public void Estoque_EstoqueValido_DeveCriarUmaInstanciaDeEstoque()
    {
        // Arrange
        var estoque = fixture.GerarEstoque();

        // Act - Assert
        estoque.Should().BeOfType<Estoque>("deve criar uma instância de estoque");
    }

    [Fact(DisplayName = "Novo estoque inválido")]
    [Trait("Category", "Domain - Estoque")]
    public void Estoque_EstoqueInvalido_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarEstoqueInvalido();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Produto inválido", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Calcular nova quantidade do estoque após entrada")]
    [Trait("Category", "Domain - Estoque")]
    public void Estoque_CalcularQuantidadeAposEntrada_DeveRetornarQuantidadeDoEstoque()
    {
        // Arrange
        var qtdEstoque = 6;
        var qtdEntrada = 3;
        var estoque = fixture.GerarEstoque(qtdEstoque);
        qtdEstoque += qtdEntrada;

        // Act
        estoque.AtualizarQuantidade(qtdEntrada, TipoMovimentacaoEstoque.Entrada);

        // Assert
        estoque.Quantidade.Should().Be(qtdEstoque, "deve ser igual a quantidade calculada");
    }

    [Fact(DisplayName = "Calcular nova quantidade do estoque após saída")]
    [Trait("Category", "Domain - Estoque")]
    public void Estoque_CalcularQuantidadeAposSaida_DeveRetornarQuantidadeDoEstoque()
    {
        // Arrange
        var qtdEstoque = 6;
        var qtdSaida = 3;
        var estoque = fixture.GerarEstoque(qtdEstoque);
        qtdEstoque -= qtdSaida;

        // Act
        estoque.AtualizarQuantidade(qtdSaida, TipoMovimentacaoEstoque.Saida);

        // Assert
        estoque.Quantidade.Should().Be(qtdEstoque, "deve ser igual a quantidade calculada");
    }

    [Fact(DisplayName = "Adicionar movimentação ao estoque")]
    [Trait("Category", "Domain - Estoque")]
    public void Estoque_AdicionarMovimentacao_DeveAdicionarMovimentacao()
    {
        // Arrange
        var estoque = fixture.GerarEstoque();
        var movimentacao = fixture.GerarMovimentacao(estoque.Id);

        // Act
        estoque.AdicionarMovimentacao(movimentacao);

        // Assert
        estoque.Movimentacoes.Should().Contain(movimentacao, "deve ter o item");
    }
}