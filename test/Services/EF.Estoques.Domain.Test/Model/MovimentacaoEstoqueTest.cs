using EF.Domain.Commons.DomainObjects;
using EF.Estoques.Domain.Models;
using FluentAssertions;

namespace EF.Estoques.Domain.Test.Model
{
    [Collection(nameof(EstoqueCollection))]
    public class MovimentacaoEstoqueTest(EstoqueFixture fixture)
    {
        [Fact(DisplayName = "Nova MovimentacaoEstoque válida")]
        [Trait("Category", "Domain - Estoque")]
        public void MovimentacaoEstoque_MovimentacaoEstoque_DeveCriarUmaInstanciaDeEstoque()
        {
            // Arrange
            var movimentacao = fixture.GerarMovimentacao(Guid.NewGuid());

            // Act - Assert
            movimentacao.Should().BeOfType<MovimentacaoEstoque>("deve criar uma instância de MovimentacaoEstoque");
        }

        [Fact(DisplayName = "Nova MovimentacaoEstoque com TipoMovimentacaoEstoque inválido")]
        [Trait("Category", "Domain - Estoque")]
        public void MovimentacaoEstoque_MovimentacaoEstoqueTipoMovimentacaoInvalido_DeveLancarExcecaoDominio()
        {
            // Arrange - Act
            Action act = () => fixture.GerarMovimentacao(Guid.NewGuid(), (TipoMovimentacaoEstoque)999);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("TipoMovimentacao inválido", "deve lançar a exceção de domínio");
        }

        [Fact(DisplayName = "Nova MovimentacaoEstoque com OrigemMovimentacaoEstoque inválida")]
        [Trait("Category", "Domain - Estoque")]
        public void MovimentacaoEstoque_MovimentacaoEstoqueOrigemMovimentacaoInvalida_DeveLancarExcecaoDominio()
        {
            // Arrange - Act
            Action act = () => fixture.GerarMovimentacao(Guid.NewGuid(), origemMovimentacao: (OrigemMovimentacaoEstoque)999);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("OrigemMovimentacao inválida", "deve lançar a exceção de domínio");
        }

        [Theory(DisplayName = "Nova MovimentacaoEstoque com OrigemMovimentacaoEstoque incompatível com TipoMovimentacao")]
        [Trait("Category", "Domain - Estoque")]
        [InlineData(TipoMovimentacaoEstoque.Entrada, OrigemMovimentacaoEstoque.Venda)]
        [InlineData(TipoMovimentacaoEstoque.Saida, OrigemMovimentacaoEstoque.Compra)]
        [InlineData(TipoMovimentacaoEstoque.Saida, OrigemMovimentacaoEstoque.CancelamentoVenda)]
        public void MovimentacaoEstoque_MovimentacaoEstoquIncompativelTipoMovimentacao_DeveLancarExcecaoDominio(TipoMovimentacaoEstoque tipoMovimentacao,
                                                                                                                OrigemMovimentacaoEstoque origemMovimentacao)
        {
            // Arrange - Act
            Action act = () => fixture.GerarMovimentacao(Guid.NewGuid(), tipoMovimentacao, origemMovimentacao);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("OrigemMovimentacao incompatível com TipoMovimentacao", "deve lançar a exceção de domínio");
        }
    }
}
