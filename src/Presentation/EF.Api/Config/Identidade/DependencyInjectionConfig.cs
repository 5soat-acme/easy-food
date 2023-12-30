using EF.Identidade.Application.Services;
using EF.Identidade.Application.Services.Interfaces;

namespace EF.Api.Config.Identidade;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesIdentidade(this IServiceCollection services)
    {
        // Application - Services
        services.AddScoped<IAcessoAppService, AcessoAppService>();
        return services;
    }
}