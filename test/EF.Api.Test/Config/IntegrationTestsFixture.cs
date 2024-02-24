using System.Net.Http.Json;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EF.Api.Test.Config;

[CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture>
{
}

public class IntegrationTestsFixture : IAsyncLifetime
{
    private readonly ApiFactory _apiFactory;
    private readonly DockerHelper _dockerHelper;
    public HttpClient Client;
    public RespostaTokenAcesso? RespostaTokenAcesso;

    public IntegrationTestsFixture()
    {
        _apiFactory = new ApiFactory();
        _dockerHelper = new DockerHelper();

        Client = _apiFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        });
    }

    public async Task InitializeAsync()
    {
        await _dockerHelper.StartContainerAsync();
    }

    public async Task DisposeAsync()
    {
        await _dockerHelper.RemoveContainerAsync();
        _apiFactory.Dispose();
        Client.Dispose();
    }

    public async Task AcessarApi(UsuarioAcesso? usuarioAcesso = null, bool novoAcesso = false)
    {
        if (!novoAcesso && RespostaTokenAcesso != null) return;

        var response = await Client.PostAsJsonAsync("api/identidade/acessar", usuarioAcesso);
        response.EnsureSuccessStatusCode();
        RespostaTokenAcesso = (await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>())!;
    }
}