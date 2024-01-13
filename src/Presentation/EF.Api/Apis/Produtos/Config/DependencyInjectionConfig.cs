using EF.Produtos.Application.Commands;
using EF.Produtos.Application.Mappings;
using EF.Produtos.Application.Queries;
using EF.Produtos.Application.Queries.Interfaces;
using EF.Produtos.Domain.Repository;
using EF.Produtos.Infra.Data;
using EF.Produtos.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Produtos.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesProdutos(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CriarProdutoCommand>());

        // Application - Queries
        services.AddScoped<IProdutoQuery, ProdutoQuery>();

        // Application - Mappings
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // Infra - Data
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddDbContext<ProdutoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}