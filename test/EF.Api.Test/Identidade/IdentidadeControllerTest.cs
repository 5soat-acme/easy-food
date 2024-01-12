using System.Net.Http.Json;
using Bogus;
using Bogus.Extensions.Brazil;
using EF.Api.Test.Fixtures;
using EF.Identidade.Application.DTOs.Responses;
using EF.Test.Utils.Builders.Identidade;
using FluentAssertions;

namespace EF.Api.Test.Identidade;

[Collection(nameof(IntegrationTestCollection))]
public class IdentidadeControllerTest(IntegrationTestFixture fixture)
{
    [Fact(DisplayName = "Criar usuário com sucesso")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_CriarUsuario_DeveRegistrarUsuarioComSucesso()
    {
        // Arrange
        var novoUsuario = new NovoUsuarioBuilder().Generate();

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/identidade", novoUsuario);

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();
        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        responseBody.Should().NotBeNull("deve ser diferente de null");
        responseBody?.Token.Should().NotBeNullOrEmpty("deve ser diferente de null ou vazio");
        responseBody?.User.Claims.FirstOrDefault(f => f.Type == "email")?.Value.Should().Be(novoUsuario.Email);
        //responseBody?.User.Claims.FirstOrDefault(f => f.Type == "cpf")?.Value.Should().Be(novoUsuario.Cpf);
    }

    [Fact(DisplayName = "Gera token de acesso para usuário cadastrado")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_Login_DeveGerarTokenAcesso()
    {
        // Arrange
        var usuarioLogin = fixture.ObterUsuarioTestes();

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/identidade/autenticar", usuarioLogin);

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();
        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        responseBody.Should().NotBeNull("deve ser diferente de null");
        responseBody?.Token.Should().NotBeNullOrEmpty("deve ser diferente de null ou vazio");
        responseBody?.User.Claims.FirstOrDefault(f => f.Type == "email")?.Value.Should().Be(usuarioLogin.Email);
        //responseBody?.User.Claims.FirstOrDefault(f => f.Type == "cpf")?.Value.Should().Be(novoUsuario.Cpf);
    }

    [Fact(DisplayName = "Gerar token de acesso anônimo")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_AcessarSemIdentificacao_DeveGerarTokenAcesso()
    {
        // Arrange & Act
        var response = await fixture.Client.PostAsync("api/identidade/acessar-anonimo", null);

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();
        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        responseBody.Should().NotBeNull("deve ser diferente de null");
        responseBody?.Token.Should().NotBeNullOrEmpty("deve ser diferente de null ou vazio");
        responseBody?.User.Claims.FirstOrDefault(f => f.Type == "cpf")?.Value.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "Gerar token de acesso anônimo com CPF")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_AcessarSemIdentificacaoComCpf_DeveGerarTokenAcesso()
    {
        // Arrange
        var cpf = new Faker().Person.Cpf();

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/identidade/acessar-anonimo", new { cpf });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();
        response.IsSuccessStatusCode.Should().BeTrue("deve ser true");
        responseBody.Should().NotBeNull("deve ser diferente de null");
        responseBody?.Token.Should().NotBeNullOrEmpty("deve ser diferente de null ou vazio");
        responseBody?.User.Claims.FirstOrDefault(f => f.Type == "cpf")?.Value.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "Senha incorreta para usuário cadastrado")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_LoginSenhaIncorreta_DeveRetornarErro()
    {
        // Arrange
        var usuarioLogin = fixture.ObterUsuarioTestes();
        usuarioLogin.Senha = "123456";

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/identidade/autenticar", usuarioLogin);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse("deve ser false");
    }

    [Fact(DisplayName = "Tentativa de login com email inválido")]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_LoginEmailInvalido_DeveRetornarErro()
    {
        // Arrange
        var usuarioLogin = fixture.ObterUsuarioTestes();
        usuarioLogin.Email = "email@errado.com";

        // Act
        var response = await fixture.Client.PostAsJsonAsync("api/identidade/autenticar", usuarioLogin);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse("deve ser false");
    }
}