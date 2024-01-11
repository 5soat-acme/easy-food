using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Api.Test.Config;

public class IntegrationTestAppFactory : WebApplicationFactory<Program>
{
    public IMemoryCache MemoryCache;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            var scope = services.BuildServiceProvider().CreateScope();
            MemoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        });
    }
}