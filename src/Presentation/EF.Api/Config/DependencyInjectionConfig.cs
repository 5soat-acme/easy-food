using EF.Carrinho.Application.Services;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Repository;
using EF.Carrinho.Infra.Data;
using EF.Carrinho.Infra.Data.Repository;
using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Cupons.Application.Commands;
using EF.Cupons.Application.Queries;
using EF.Cupons.Domain.Repository;
using EF.Cupons.Infra;
using EF.Cupons.Infra.Data.Repository;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations.CarrinhoIntegracao;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.Queries;
using EF.Estoques.Domain.Repository;
using EF.Estoques.Infra;
using EF.Estoques.Infra.Data.Repository;
using EF.Identidade.Application.Services;
using EF.Identidade.Application.Services.Interfaces;
using EF.Pedidos.Application.Commands;
using EF.Pedidos.Application.Mappings;
using EF.Pedidos.Application.Queries;
using EF.Pedidos.Application.Queries.Interfaces;
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
        RegisterServicesEstoques(services, configuration);
        RegisterServicesCupons(services, configuration);

        return services;
    }

    private static void RegisterServicesPedidos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarPedidoCommand, CommandResult>, CriarPedidoCommandHandler>();

        // Application - Queries
        services.AddScoped<IPedidoQuery, PedidoQuery>();
        
        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));

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
        services.AddScoped<ICarrinhoService, CarrinhoService>();
        services.AddScoped<INotificationHandler<CarrinhoFechadoEvent>, IntegraPedidoService>();

        // Application - Mapping
        services.AddAutoMapper(typeof(Carrinho.Application.Mappings.DomainToDtoProfile));

        // Infra - Data
        services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
        services.AddDbContext<CarrinhoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
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

    private static void RegisterServicesCupons(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarCupomCommand, CommandResult>, CriarCupomCommandHandler>();
        services
            .AddScoped<IRequestHandler<AtualizarCupomCommand, CommandResult>, AtualizarCupomCommandHandler>();
        services
            .AddScoped<IRequestHandler<InativarCupomCommand, CommandResult>, InativarCupomCommandHandler>();
        services
            .AddScoped<IRequestHandler<RemoverProdutosCommand, CommandResult>, RemoverProdutosCommandHandler>();
        services
            .AddScoped<IRequestHandler<InserirProdutosCommand, CommandResult>, InserirProdutosCommandHandler>();

        // Application - Queries
        services.AddScoped<ICupomQuery, CupomQuery>();

        // Infra - Data 
        services.AddScoped<ICupomRepository, CupomRepository>();
        services.AddDbContext<CupomDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}