using System.Net.Http.Json;
using EF.Api.Test.Fixtures;
using EF.Carrinho.Application.DTOs.Responses;
using EF.Test.Utils.Builders.Carrinho;
using FluentAssertions;

namespace EF.Api.Test.Carrinho;

[Collection(nameof(IntegrationTestCollection))]
public class CarrinhoControllerTest(IntegrationTestFixture fixture)
{
    [Fact(DisplayName = "Obter carrinho")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_ObterCarrinho_DeveRetornarCarrinhoCliente()
    {
        // Arrange
        fixture.AdicionarTokenRequest();

        // Act
        var response = await fixture.Client.GetAsync("api/carrinho");

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<CarrinhoClienteDto>();
        responseBody.Should().NotBeNull("deve ser diferente de nulo");
    }

    [Fact(DisplayName = "Adicionar item ao carrinho")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_AdicionarItem_ItemDeveSerAdicionadoComSucesso()
    {
        // Arrange
        fixture.AdicionarTokenRequest();
        var item = new AdicionarItemDtoBuilder().Generate();

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/carrinho", item);

        // Assert
        var carrinhoResponse = await fixture.Client.GetAsync("api/carrinho");
        var carrinho = await carrinhoResponse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();
        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        carrinho.Itens.Should().Contain(x => x.ProdutoId == item.ProdutoId && x.Quantidade == item.Quantidade,
            "a quantidade do item deve ser igual ao informado");
    }

    [Fact(DisplayName = "Adicionar item existente ao carrinho")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_AdicionarItemExistente_QuntidadeItemDeveSerAtualizada()
    {
        // Arrange
        fixture.AdicionarTokenRequest();
        var item = new AdicionarItemDtoBuilder().Generate();

        // Act
        var response1 = await fixture.Client.PostAsJsonAsync("api/carrinho", item);
        var response2 = await fixture.Client.PostAsJsonAsync("api/carrinho", item);

        // Assert
        var carrinhoResponse = await fixture.Client.GetAsync("api/carrinho");
        var carrinho = await carrinhoResponse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();
        response1.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        response2.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        carrinho.Itens.Should()
            .Contain(x => x.ProdutoId == item.ProdutoId && x.Quantidade == item.Quantidade * 2);
    }

    [Fact(DisplayName = "Atualizar a quantidade de um item")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_AtualizarItem_QuantidadeItemDeveSerAtualizada()
    {
        // Arrange
        fixture.AdicionarTokenRequest();
        var item = new AdicionarItemDtoBuilder().Generate();
        await fixture.Client.PostAsJsonAsync("api/carrinho", item);
        var carrinhoResonse = await fixture.Client.GetAsync("api/carrinho");
        var carrinho = await carrinhoResonse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();
        var itemAdicionado = carrinho.Itens.FirstOrDefault(x => x.ProdutoId == item.ProdutoId);
        var itemAtualizar = new AtualizarItemDtoBuilder()
            .ItemId(itemAdicionado.Id)
            .Generate();

        // Act
        var response = await fixture.Client.PutAsJsonAsync($"api/carrinho/{itemAtualizar.ItemId}", itemAtualizar);

        // Assert
        var carrinhoAtualizadoResonse = await fixture.Client.GetAsync("api/carrinho");
        var carrinhoAtualizado = await carrinhoAtualizadoResonse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();

        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        carrinhoAtualizado.Itens.Should()
            .Contain(x => x.Id == itemAdicionado.Id && x.Quantidade == itemAtualizar.Quantidade,
                "a quantidade do item deve ser atualizada");
    }

    [Fact(DisplayName = "Remover item do carrinho")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_RemoverItem_ItemDeveSerRemovidoDoCarrinho()
    {
        // Arrange
        fixture.AdicionarTokenRequest();
        var item = new AdicionarItemDtoBuilder().Generate();
        await fixture.Client.PostAsJsonAsync("api/carrinho", item);
        var carrinhoResonse = await fixture.Client.GetAsync("api/carrinho");
        var carrinho = await carrinhoResonse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();
        var itemAdicionado = carrinho.Itens.FirstOrDefault(x => x.ProdutoId == item.ProdutoId);

        // Act
        var response = await fixture.Client.DeleteAsync($"api/carrinho/{itemAdicionado.Id}");

        // Assert
        var carrinhoAtualizadoResonse = await fixture.Client.GetAsync("api/carrinho");
        var carrinhoAtualizado = await carrinhoAtualizadoResonse.Content.ReadFromJsonAsync<CarrinhoClienteDto>();

        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        carrinhoAtualizado.Itens.Should()
            .NotContain(x => x.Id == itemAdicionado.Id,
                "o item não deve mais existir no carrinho");
    }
}