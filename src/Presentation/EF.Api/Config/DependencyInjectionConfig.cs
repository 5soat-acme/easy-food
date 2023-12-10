using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Domain.Commons.Mediator;
using EF.Identidade.Application.Services;
using EF.Pedidos.Application.Commands;
using EF.Pedidos.Domain.Repository;
using EF.Pedidos.Infra.Data;
using EF.Pedidos.Infra.Data.Repository;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        RegisterServicesPedidos(services, configuration);
        RegisterServicesClientes(services, configuration);
        RegisterServicesIdentidade(services, configuration);

        return services;
    }

    private static void RegisterServicesPedidos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<IncluirItemPedidoCommand, ValidationResult>, IncluirItemPedidoCommandHandler>();

        // Infra - Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddDbContext<PedidoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void RegisterServicesClientes(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarClienteCommand, ValidationResult>, CriarClienteCommandHandler>();

        // Infra - Data
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddDbContext<ClienteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void RegisterServicesIdentidade(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services.AddScoped<IAcessoAppService, AcessoAppService>();
    }
}