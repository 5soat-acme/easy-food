using EF.Pagamentos.Application.Commands;
using EF.Pagamentos.Domain.Repository;
using EF.Pagamentos.Infra;
using EF.Pagamentos.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Pagamentos.Config;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesPagamentos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AutorizarPagamentoCommandHandler>());

        // Infra - Data 
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddDbContext<PagamentoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}