using EF.Carrinho.Application.Adapters;
using EF.Carrinho.Application.Mappings;
using EF.Carrinho.Application.Ports;
using EF.Carrinho.Application.Services;
using EF.Carrinho.Application.Services.Integrations;
using EF.Carrinho.Application.Services.Interfaces;
using EF.Carrinho.Domain.Repository;
using EF.Carrinho.Infra.Data;
using EF.Carrinho.Infra.Data.Repository;
using EF.Domain.Commons.Messages.Integrations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Carrinho.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesCarrinho(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Services
        services.AddScoped<ICarrinhoConsultaService, CarrinhoConsultaService>();
        services.AddScoped<ICarrinhoManipulacaoService, CarrinhoManipulacaoService>();
        services.AddScoped<INotificationHandler<PedidoCriadoEvent>, CarrinhoIntegracaoService>();

        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));
        services.AddAutoMapper(typeof(AdapterToDtoProfile));

        // Application - Ports & Adapters
        services.AddScoped<IEstoqueService, EstoqueAdapter>();
        services.AddScoped<IProdutoService, ProdutoAdapter>();

        // Infra - Data
        services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
        services.AddDbContext<CarrinhoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}