using EF.Identidade.Application.UseCases;
using EF.Identidade.Application.UseCases.Interfaces;

namespace EF.Api.Apis.Identidade.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesIdentidade(this IServiceCollection services)
    {
        // Application - UseCases
        services.AddScoped<IAcessoUseCase, AcessoUseCase>();
        return services;
    }
}