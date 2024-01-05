using System.Net.Http.Json;
using Bogus;
using Bogus.Extensions.Brazil;
using EF.Api.Test.Fixtures;
using EF.Identidade.Application.DTOs.Responses;
using EF.Test.Utils.Orderers;
using FluentAssertions;

namespace EF.Api.Test.Identidade;

[TestCaseOrderer(
    ordererTypeName: "EF.Test.Utils.Orderers.PriorityOrderer",
    ordererAssemblyName: "EF.Test.Utils")]
[Collection(nameof(IntegrationTestCollection))]
public class IdentidadeControllerTest(IntegrationTestFixture fixture)
{
    [Fact(DisplayName = "Criar usuário com sucesso"), TestPriority(-1)]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_CriarUsuario_DeveRegistrarUsuarioComSucesso()
    {
        // Arrange
        var novoUsuario = fixture.GerarNovoUsuario();

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

    [Fact(DisplayName = "Gera token de acesso para usuário cadastrado"), TestPriority(0)]
    [Trait("Category", "EF.Identidade.Api")]
    public async Task IdentidadeController_Login_DeveGerarTokenAcesso()
    {
        // Arrange
        var usuarioLogin = fixture.ObterUsuarioLogin();

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

    [Fact(DisplayName = "Gerar token de acesso anônimo"), TestPriority(0)]
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

    [Fact(DisplayName = "Gerar token de acesso anônimo com CPF"), TestPriority(0)]
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
        responseBody?.User.Claims.FirstOrDefault(f => f.Type == "cpf")?.Value.Should().BeNullOrEmpty();
    }
}