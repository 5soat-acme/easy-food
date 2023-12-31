﻿using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Test.Fixtures;
using EF.Domain.Commons.DomainObjects;
using FluentAssertions;

namespace EF.Cupons.Domain.Test.Models;

[Collection(nameof(CupomCollection))]
public class CupomProdutoTest(CupomFixture fixture)
{
    [Fact(DisplayName = "Novo CupomProduto válido")]
    [Trait("Category", "Domain - Cupom")]
    public void CupomProduto_CupomValido_DeveCriarUmaInstanciaDeCupomProduto()
    {
        // Arrange
        var movimentacao = fixture.GerarCupomProduto(Guid.NewGuid());

        // Act - Assert
        movimentacao.Should().BeOfType<CupomProduto>("deve criar uma instância de CupomProduto");
    }

    [Fact(DisplayName = "Novo CupomProduto inválido")]
    [Trait("Category", "Domain - Cupom")]
    public void CupomProduto_CupomInvalido_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupomProduto(Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Um CupomProduto deve estar associado a um Cupom", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Novo CupomProduto com produto Id inválido")]
    [Trait("Category", "Domain - Cupom")]
    public void CupomProduto_ProdutoIdInvalido_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupomProduto(Guid.NewGuid(), Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Produto inválido", "deve lançar a exceção de domínio");
    }
}