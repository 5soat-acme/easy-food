using EF.Core.Commons.Messages;
using EF.Core.Commons.Messages.Integrations;
using EF.Pedidos.Application.Events;
using EF.Pedidos.Application.Gateways;
using EF.Pedidos.Application.Mappings;
using EF.Pedidos.Application.UseCases;
using EF.Pedidos.Application.UseCases.Interfaces;
using EF.Pedidos.Domain.Repository;
using EF.Pedidos.Infra.Adapters.Cupons;
using EF.Pedidos.Infra.Adapters.Estoque;
using EF.Pedidos.Infra.Adapters.Pagamentos;
using EF.Pedidos.Infra.Adapters.Produtos;
using EF.Pedidos.Infra.Data;
using EF.Pedidos.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Pedidos.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesPedidos(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - UseCases
        services.AddScoped<IAtualizarPedidoUseCase, AtualizarPedidoUseCase>();
        services.AddScoped<IConsultarPedidoUseCase, ConsultarPedidoUseCase>();
        services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
        services.AddScoped<IProcessarPagamentoUseCase, ProcessarPagamentoUseCase>();

        // Application - Events
        services.AddScoped<IEventHandler<PreparoPedidoIniciadoEvent>, PedidoEventHandler>();
        services.AddScoped<IEventHandler<PreparoPedidoFinalizadoEvent>, PedidoEventHandler>();
        services.AddScoped<IEventHandler<EntregaRealizadaEvent>, PedidoEventHandler>();

        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));
        services.AddAutoMapper(typeof(CumpomToDomainProfile));
        services.AddAutoMapper(typeof(ProdutoToDomainProfile));

        // Application - Gateways & Gateways
        services.AddScoped<IEstoqueService, EstoqueAdapter>();
        services.AddScoped<ICupomService, CupomAdapter>();
        services.AddScoped<IProdutoService, ProdutoAdapter>();
        services.AddScoped<IPagamentoService, PagamentoAdapter>();

        // Infra - Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddDbContext<PedidoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}