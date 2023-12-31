using EF.Cupons.Application.Commands;
using EF.Cupons.Application.Mappings;
using EF.Cupons.Application.Queries;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Cupons.Domain.Repository;
using EF.Cupons.Infra;
using EF.Cupons.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Apis.Cupons.Config;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesCupons(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CriarCupomCommandHandler>());

        // Application - Mapping
        services.AddAutoMapper(typeof(CupomDomainToDtoProfile));

        // Application - Queries
        services.AddScoped<ICupomQuery, CupomQuery>();

        // Infra - Data 
        services.AddScoped<ICupomRepository, CupomRepository>();
        services.AddDbContext<CupomDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}