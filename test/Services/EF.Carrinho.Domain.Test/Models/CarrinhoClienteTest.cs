using EF.Carrinho.Domain.Models;
using EF.Carrinho.Domain.Test.Fixtures;
using EF.Domain.Commons.DomainObjects;
using FluentAssertions;

namespace EF.Carrinho.Domain.Test.Models;

[Collection(nameof(CarrinhoClienteCollection))]
public class CarrinhoClienteTest
{
    private readonly CarrinhoClienteFixture _carrinhoClienteFixture;

    public CarrinhoClienteTest(CarrinhoClienteFixture carrinhoClienteFixture)
    {
        _carrinhoClienteFixture = carrinhoClienteFixture;
    }

    [Fact(DisplayName = "Criar carrinho associado ao cliente")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_CriarCarrinhoAssociadoCliente_DeveCriarOCarrinhoAssociadoAoCliente()
    {
        // Arrange
        var carrinhoCliente = new CarrinhoCliente(Guid.NewGuid());

        // Act & Assert 
        carrinhoCliente.ClienteId.Should().NotBeEmpty("o cliente deve estar associado ao carrinho");
    }

    [Fact(DisplayName = "Criar carrinho associado ao cliente inválido")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_CriarCarrinhoAssociadoClienteInvalido_DeveRetornarDomainException()
    {
        // Arrange & Act & Assert 
        Assert.Throws<DomainException>(() => new CarrinhoCliente(Guid.Empty));
    }

    [Fact(DisplayName = "Associar cliente inválido ao carrinho")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_AssociarClienteInvalido_RetornaDomainException()
    {
        // Arrange
        var carrinhoCliente = _carrinhoClienteFixture.ObterCarrinhoNovo();

        // Act & Assert 
        Assert.Throws<DomainException>(() => carrinhoCliente.AssociarCliente(Guid.Empty));
    }

    [Fact(DisplayName = "Associar carrinho existente inválido")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_AssociarCarrinhoExistenteInvalido_RetornaDomainException()
    {
        // Arrange
        var carrinhoCliente = _carrinhoClienteFixture.ObterCarrinhoNovo();

        // Act & Assert 
        Assert.Throws<DomainException>(() => carrinhoCliente.AssociarCarrinho(Guid.Empty));
    }

    [Fact(DisplayName = "Adicionar item ao carrinho")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_AdicionarItem_DeveAdicionarItemCarrinho()
    {
        // Arrange
        var carrinhoCliente = _carrinhoClienteFixture.ObterCarrinhoNovo();
        var item = _carrinhoClienteFixture.GerarItemValido();

        // Act
        carrinhoCliente.AdicionarItem(item);

        // Assert 
        carrinhoCliente.Itens.Should().Contain(item, "item deve ser adicionado ao carrinho");
        carrinhoCliente.ValorTotal.Should().Be(item.ValorUnitario * item.Quantidade,
            "valor total deve ser calculado corretamente");
    }

    [Fact(DisplayName = "Remover item ao carrinho")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_RemoverItem_DeveRemoverItemCarrinho()
    {
        // Arrange
        var carrinhoCliente = _carrinhoClienteFixture.ObterCarrinhoNovo();
        var item = _carrinhoClienteFixture.GerarItemValido();
        carrinhoCliente.AdicionarItem(item);

        // Act
        carrinhoCliente.RemoverItem(item);

        // Assert 
        carrinhoCliente.Itens.Should().NotContain(item, "item deve ser removido do carrinho");
        carrinhoCliente.ValorTotal.Should().Be(0, "valor total deve ser recalculado corretamente");
    }

    [Fact(DisplayName = "Atualizar valor total do carrinho")]
    [Trait("Category", "Carrinho.Domain.CarrinhoCliente")]
    public void CarrinhoCliente_AtualizarValorTotal_DeveRetornarValorTotal()
    {
        // Arrange
        var carrinhoCliente = _carrinhoClienteFixture.ObterCarrinhoNovo();
        var itens = _carrinhoClienteFixture.GerarItensValidos(10);
        decimal valorTotal = 0;
        foreach (var item in itens)
        {
            carrinhoCliente.AdicionarItem(item);
            valorTotal += item.ValorUnitario * item.Quantidade;
        }

        // Act
        carrinhoCliente.CalcularValorTotal();

        // Assert
        carrinhoCliente.ValorTotal.Should().Be(valorTotal, "valor total deve ser calculado corretamente");
        carrinhoCliente.ValorTotal.Should()
            .Be(carrinhoCliente.ValorTotal, "propriedade deve ser atualizada corretamente");
    }
}