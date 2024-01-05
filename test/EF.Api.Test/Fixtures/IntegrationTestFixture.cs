using EF.Api.Test.Config;
using EF.Identidade.Application.DTOs.Requests;
using EF.Test.Utils.Builders.Identidade;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Api.Test.Fixtures;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}

public class IntegrationTestFixture : IDisposable
{
    public readonly IntegrationTestAppFactory AppFactory;
    public readonly HttpClient Client;
    private readonly IMemoryCache _memoryCache;

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

    public NovoUsuario GerarNovoUsuario()
    {
        var novoUsuario = new NovoUsuarioBuilder().Generate();

        if (GetCacheItem("UsuarioLogin") is null)
        {
            SetCacheItem("UsuarioLogin", new UsuarioLogin
            {
                Email = novoUsuario.Email,
                Senha = novoUsuario.Senha
            });
        }

        return novoUsuario;
    }

    public UsuarioLogin? ObterUsuarioLogin()
    {
        var cache = GetCacheItem("UsuarioLogin");
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
        if (_memoryCache.TryGetValue(key, out object? value))
        {
            return value;
        }

        return null;
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}