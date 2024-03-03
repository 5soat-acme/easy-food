using EF.Identidade.Application.UseCases;
using EF.Identidade.Application.UseCases.Interfaces;
using EF.Identidade.Domain.Services;
using EF.Identidade.Infra.Services;

namespace EF.Api.Apis.Identidade.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesIdentidade(this IServiceCollection services)
    {
        // Application - UseCases
        services.AddScoped<IIdentidadeUseCase, IdentidadeUseCase>();

        // Domain
        services.AddScoped<IUsuarioService, UsuarioService>();
        return services;
    }
}