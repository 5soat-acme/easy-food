using System.Net.Http.Json;
using EF.Api.Test.Config;
using EF.Commons.Test.Builders.Carrinho;
using EF.Commons.Test.Extensions;

namespace EF.Api.Test.Controllers;

[Collection(nameof(IntegrationTestsFixtureCollection))]
public class CarrinhoControllerTest
{
    private readonly IntegrationTestsFixture _testsFixture;

    public CarrinhoControllerTest(IntegrationTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Adicionar item no carrinho vazio")]
    [Trait("Category", "API - Carrinho")]
    public async Task AdicionarItem_CarrinhoVazio_DeveRetornarSucesso()
    {
        // Arrange
        var produto = new AdicionarItemDtoBuilder().Generate();
        await _testsFixture.AcessarApi();
        _testsFixture.Client.AddToken(_testsFixture.RespostaTokenAcesso!.Token);

        // Act
        var response = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", produto);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}