using EF.Pagamentos.Application.Commands;
using EF.Pagamentos.Application.Mappings;
using EF.Pagamentos.Application.Queries;
using EF.Pagamentos.Application.Queries.Interfaces;
using EF.Pagamentos.Domain.Repository;
using EF.Pagamentos.Infra;
using EF.Pagamentos.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Pagamentos.Config;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesPagamentos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CriarPagamentoCommandHandler>());

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