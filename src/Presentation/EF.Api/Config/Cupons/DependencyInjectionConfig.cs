using EF.Cupons.Application.Commands;
using EF.Cupons.Application.Mappings;
using EF.Cupons.Application.Queries;
using EF.Cupons.Application.Queries.Interfaces;
using EF.Cupons.Domain.Repository;
using EF.Cupons.Infra;
using EF.Cupons.Infra.Data.Repository;
using EF.Domain.Commons.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config.Cupons;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesCupons(this IServiceCollection services, IConfiguration configuration)
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
}