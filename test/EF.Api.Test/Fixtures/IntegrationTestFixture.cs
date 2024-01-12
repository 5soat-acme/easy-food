using System.Net.Http.Headers;
using System.Net.Http.Json;
using EF.Api.Test.Config;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Test.Utils.Builders.Identidade;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;

namespace EF.Api.Test.Fixtures;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}

public class IntegrationTestFixture : IAsyncLifetime
{
    private const string TokenUserTest = "TokenUserTest";
    private const string UserTest = "UserTest";
    private readonly IMemoryCache _memoryCache;
    public readonly IntegrationTestAppFactory AppFactory;
    public readonly HttpClient Client;

    public IntegrationTestFixture()
    {
        AppFactory = new IntegrationTestAppFactory();

        Client = AppFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            HandleCookies = true,
            MaxAutomaticRedirections = 7,
            BaseAddress = new Uri("https://localhost:5002")
        });

        _memoryCache = AppFactory.MemoryCache;
    }

    public async Task InitializeAsync()
    {
        await GerarUsuarioTestes();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        _memoryCache.Dispose();
    }

    public RespostaTokenAcesso? ObterTokenTestes()
    {
        var cache = GetCacheItem(TokenUserTest);
        return cache as RespostaTokenAcesso;
    }

    public void AdicionarTokenRequest(string? token = null)
    {
        if (string.IsNullOrEmpty(token))
        {
            var tokenResposta = ObterTokenTestes();
            token = tokenResposta.Token;
        }

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void RemoverTokenRequest()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }

    public UsuarioLogin? ObterUsuarioTestes()
    {
        var cache = GetCacheItem(UserTest);
        return cache as UsuarioLogin;
    }

    public void SetCacheItem(string key, object value)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
            SlidingExpiration = TimeSpan.FromMinutes(60)
        };

        _memoryCache.Set(key, value, options);
    }

    public object? GetCacheItem(string key)
    {
        if (_memoryCache.TryGetValue(key, out var value)) return value;

        return null;
    }

    private async Task GerarUsuarioTestes()
    {
        var novoUsuario = new NovoUsuarioBuilder().Generate();

        SetCacheItem(UserTest, new UsuarioLogin
        {
            Email = novoUsuario.Email,
            Senha = novoUsuario.Senha
        });

        var response = await Client.PostAsJsonAsync("api/identidade", novoUsuario);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<RespostaTokenAcesso>();

        SetCacheItem(TokenUserTest, responseBody!);
    }
}