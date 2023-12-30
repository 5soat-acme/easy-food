using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Domain.Commons.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config.Clientes;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesClientes(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarClienteCommand, CommandResult>, CriarClienteCommandHandler>();

        // Infra - Data
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddDbContext<ClienteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}