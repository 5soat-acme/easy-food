using EF.Carrinho.Application.Mapping;
using EF.Carrinho.Application.Services;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Repository;
using EF.Carrinho.Infra.Data;
using EF.Carrinho.Infra.Data.Repository;
using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations.CarrinhoIntegracao;
using EF.Identidade.Application.Services;
using EF.Identidade.Application.Services.Interfaces;
using EF.Pedidos.Application.Commands;
using EF.Pedidos.Application.Services;
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
        RegisterServicesCarrinho(services, configuration);

        return services;
    }

    private static void RegisterServicesPedidos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<GerarPedidoCommand, CommandResult>, GerarPedidoCommandHandler>();

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
        // Application - Services
        services.AddScoped<IAcessoAppService, AcessoAppService>();
    }
    
    private static void RegisterServicesCarrinho(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Services
        services.AddScoped<ICarrinhoAppService, CarrinhoAppService>();
        services.AddScoped<INotificationHandler<CarrinhoFechadoEvent>, IntegraPedidoService>();
        
        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));
        
        // Infra - Data
        services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
        services.AddDbContext<CarrinhoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}