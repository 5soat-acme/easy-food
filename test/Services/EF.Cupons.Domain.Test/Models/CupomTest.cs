using EF.Cupons.Domain.Models;
using EF.Cupons.Domain.Test.Fixtures;
using EF.Domain.Commons.DomainObjects;
using FluentAssertions;

namespace EF.Cupons.Domain.Test.Models;

[Collection(nameof(CupomCollection))]
public class CupomTest(CupomFixture fixture)
{
    [Fact(DisplayName = "Novo cupom válido")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_CupomValido_DeveCriarUmaInstanciaDeCupom()
    {
        // Arrange
        var cupom = fixture.GerarCupom();

        // Act - Assert
        cupom.Should().BeOfType<Cupom>("deve criar uma instância de cupom");
    }

    [Fact(DisplayName = "Inativar cupom")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_InativarCupom_DeveRetornarCupomInativo()
    {
        // Arrange
        var cupom = fixture.GerarCupom();

        // Act
        cupom.InativarCupom();

        // Assert
        cupom.Status.Should().Be(CupomStatus.Inativo, "deve ser igual a inativo");
    }

    [Fact(DisplayName = "Novo cupom com data início inválida")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_CupomDataInicioInvalida_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupom(DateTime.Now.AddDays(-1));

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("DataInicio não pode ser inferior a data atual", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Novo cupom com data fim inválida")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_CupomDataFimInvalida_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupom(DateTime.Now.AddDays(5), DateTime.Now.AddDays(4));

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("DataFim não pode ser inferior a DataInicio", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Novo cupom com código inválido")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_CodigoInvalido_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupom(codigoCupom: "");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CodigoCupom inválido", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Novo cupom com porcentagem de desconto inválida")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_PorcentagemDescontoInvalida_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupom(porcentagemDesconto: 0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("PorcentagemDesconto inválida", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Novo cupom com status inválido")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_StatusInvalido_DeveLancarExcecaoDominio()
    {
        // Arrange - Act
        Action act = () => fixture.GerarCupom(status: (CupomStatus)999);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Status inválido", "deve lançar a exceção de domínio");
    }

    [Fact(DisplayName = "Adicionar produto ao cupom")]
    [Trait("Category", "Domain - Cupom")]
    public void Cupom_AdicionarProduto_DeveAdicionarProduto()
    {
        // Arrange
        var cupom = fixture.GerarCupom();
        var cupomProd = fixture.GerarCupomProduto(cupom.Id);

        // Act
        cupom.AdicionarProduto(cupomProd);

        // Assert
        cupom.CupomProdutos.Should().Contain(cupomProd, "deve ter o produto");
    }
}