using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.Queries;
using EF.Estoques.Domain.Repository;
using EF.Estoques.Infra;
using EF.Estoques.Infra.Data.Repository;
using EF.Identidade.Application.Services;
using EF.Identidade.Application.Services.Interfaces;
using EF.Pedidos.Application.Commands;
using EF.Pedidos.Domain.Repository;
using EF.Pedidos.Infra.Data;
using EF.Pedidos.Infra.Data.Repository;
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
        RegisterServicesEstoques(services, configuration);

        return services;
    }

    private static void RegisterServicesPedidos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<IncluirItemPedidoCommand, CommandResult>, IncluirItemPedidoCommandHandler>();

        // Infra - Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddDbContext<PedidoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void RegisterServicesClientes(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarClienteCommand, CommandResult>, CriarClienteCommandHandler>();

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

    private static void RegisterServicesEstoques(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<AtualizarEstoqueCommand, CommandResult>, AtualizarEstoqueCommandHandler>();

        // Application - Queries
        services.AddScoped<IEstoqueQuery, EstoqueQuery>();

        // Infra - Data 
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddDbContext<EstoqueDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}