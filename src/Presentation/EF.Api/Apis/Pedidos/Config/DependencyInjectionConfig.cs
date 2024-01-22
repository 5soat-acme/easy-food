using EF.Pedidos.Application.Mappings;
using EF.Pedidos.Application.Ports;
using EF.Pedidos.Application.Queries;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.Pedidos.Application.Services.Integrations;
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
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PedidoIntegrationService>());

        // Application - Queries
        services.AddScoped<IPedidoQuery, PedidoQuery>();

        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));
        services.AddAutoMapper(typeof(CumpomToDomainProfile));
        services.AddAutoMapper(typeof(ProdutoToDomainProfile));

        // Application - Ports & Adapters
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