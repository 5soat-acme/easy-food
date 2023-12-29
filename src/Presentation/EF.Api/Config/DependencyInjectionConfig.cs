using EF.Carrinho.Application.Mappings;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Repository;
using EF.Carrinho.Infra.Data;
using EF.Carrinho.Infra.Data.Repository;
using EF.Carrinho.Infra.Integrations;
using EF.Clientes.Application.Commands;
using EF.Clientes.Domain.Repository;
using EF.Clientes.Infra.Data;
using EF.Clientes.Infra.Data.Repository;
using EF.Cupons.Application.Commands;
using EF.Cupons.Application.Mappings;
using EF.Cupons.Application.Queries;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Cupons.Domain.Repository;
using EF.Cupons.Infra;
using EF.Cupons.Infra.Data.Repository;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.Mappings;
using EF.Estoques.Application.Queries;
using EF.Estoques.Application.Queries.Interfaces;
using EF.Estoques.Domain.Repository;
using EF.Estoques.Infra;
using EF.Estoques.Infra.Data.Repository;
using EF.Identidade.Application.Services;
using EF.Identidade.Application.Services.Interfaces;
using EF.Pagamentos.Application.Commands;
using EF.Pagamentos.Application.Mappings;
using EF.Pagamentos.Application.Queries;
using EF.Pagamentos.Application.Queries.Interfaces;
using EF.Pagamentos.Domain.Repository;
using EF.Pagamentos.Infra;
using EF.Pagamentos.Infra.Data.Repository;
using EF.Pedidos.Application.Commands.Recebimento;
using EF.Pedidos.Application.Queries;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.Pedidos.Application.Services;
using EF.Pedidos.Domain.Repository;
using EF.Pedidos.Infra.Data;
using EF.Pedidos.Infra.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainToDtoProfile = EF.Pedidos.Application.Mappings.DomainToDtoProfile;

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
        RegisterServicesPagamentos(services, configuration);

        return services;
    }

    private static void RegisterServicesPedidos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<ReceberPedidoCommand, CommandResult>, ReceberPedidoCommandHandler>();

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
        services.AddScoped<ICarrinhoConsultaService, CarrinhoConsultaService>();
        services.AddScoped<ICarrinhoManipulacaoService, CarrinhoManipulacaoService>();
        services.AddScoped<ICarrinhoCheckoutService, CarrinhoCheckoutService>();
        services.AddScoped<INotificationHandler<PedidoRecebidoEvent>, IntegrarPedidoService>();

        // Application - Ports
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<ICupomService, CupomService>();

        // Application - Mapping
        services.AddAutoMapper(typeof(Carrinho.Application.Mappings.DomainToDtoProfile));
        services.AddAutoMapper(typeof(DtoToDomainProfile));
        services.AddAutoMapper(typeof(ExternalDtoToDtoProfile));

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

        // Application - Mapping
        services.AddAutoMapper(typeof(EstoqueDomainToDtoProfile));

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

        // Application - Mapping
        services.AddAutoMapper(typeof(CupomDomainToDtoProfile));

        // Application - Queries
        services.AddScoped<ICupomQuery, CupomQuery>();

        // Infra - Data 
        services.AddScoped<ICupomRepository, CupomRepository>();
        services.AddDbContext<CupomDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void RegisterServicesPagamentos(IServiceCollection services, IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarPagamentoCommand, CommandResult>, CriarPagamentoCommandHandler>();

        // Application - Mapping
        services.AddAutoMapper(typeof(PagamentoDomainToDtoProfile));

        // Application - Queries
        services.AddScoped<IFormaPagamentoQuery, FormaPagamentoQuery>();

        // Infra - Data 
        services.AddScoped<IFormaPagamentoRepository, FormaPagamentoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddDbContext<PagamentoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}