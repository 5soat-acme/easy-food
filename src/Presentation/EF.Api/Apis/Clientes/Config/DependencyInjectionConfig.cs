using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Clientes.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesClientes(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CriarClienteCommandHandler>());

        // Infra - Data
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddDbContext<ClienteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}