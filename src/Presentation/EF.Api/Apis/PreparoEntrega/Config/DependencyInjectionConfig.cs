using EF.PreparoEntrega.Application.Mapping;
using EF.PreparoEntrega.Application.Queries;
using EF.PreparoEntrega.Application.Queries.Interfaces;
using EF.PreparoEntrega.Application.Services.Integrations;
using EF.PreparoEntrega.Domain.Repository;
using EF.PreparoEntrega.Infra.Data;
using EF.PreparoEntrega.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.PreparoEntrega.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesPreparoEntrega(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PedidoIntegrationService>());

        // Application - Queries
        services.AddScoped<IPreparoEntregaQuery, PreparoEntregaQuery>();

        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // Infra - Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddDbContext<PreparoEntregaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}