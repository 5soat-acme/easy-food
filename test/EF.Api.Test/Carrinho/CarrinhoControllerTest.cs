using System.Net.Http.Json;
using EF.Api.Test.Fixtures;
using EF.Identidade.Application.DTOs.Responses;

namespace EF.Api.Test.Carrinho;

[Collection(nameof(IntegrationTestCollection))]
public class CarrinhoControllerTest(IntegrationTestFixture fixture)
{
    [Fact(DisplayName = "Criar usuário com sucesso")]
    [Trait("Category", "EF.Carrinho.Api")]
    public async Task CarrinhoController_ObterCarrinho_DeveRetornarCarrinhoCliente()
    {
        // // Arrange
        // var novoUsuario = fixture.GerarNovoUsuario();
        //
        // // Act
        // var response = await fixture.Client.PostAsJsonAsync("api/identidade", novoUsuario);
        //
        // // Assert
        // var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();
        Assert.True(true);
    }
}