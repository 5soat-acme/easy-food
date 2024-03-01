using System.Net.Http.Json;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq.AutoMock;

namespace EF.Api.Test.Config;

[CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture>
{
}

public class IntegrationTestsFixture : IAsyncLifetime
{
    public readonly ApiFactory ApiFactory;
    private readonly DockerHelper _dockerHelper;
    public HttpClient Client;
    public RespostaTokenAcesso? RespostaTokenAcesso;
    public AutoMocker Mocker;

    public IntegrationTestsFixture()
    {
        ApiFactory = new ApiFactory();
        _dockerHelper = new DockerHelper();

        Client = ApiFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });
        Mocker = new AutoMocker();
    }

    public async Task InitializeAsync()
    {
        //await _dockerHelper.StartContainerAsync();
    }

    public async Task DisposeAsync()
    {
        //await _dockerHelper.RemoveContainerAsync();
        ApiFactory.Dispose();
        Client.Dispose();
    }

    public async Task AcessarApi(UsuarioAcesso? usuarioAcesso = null, bool novoAcesso = false)
    {
        if (!novoAcesso && RespostaTokenAcesso != null) return;

        var response = await Client.PostAsJsonAsync("api/identidade/acessar", usuarioAcesso);
        response.EnsureSuccessStatusCode();
        RespostaTokenAcesso = (await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>())!;
    }

    public void ObterProdutos()
    {
        throw new NotImplementedException();
    }
}