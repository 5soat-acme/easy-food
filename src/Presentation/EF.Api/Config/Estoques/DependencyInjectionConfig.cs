using EF.Domain.Commons.Messages;
using EF.Estoques.Application.Commands;
using EF.Estoques.Application.Mappings;
using EF.Estoques.Application.Queries;
using EF.Estoques.Application.Queries.Interfaces;
using EF.Estoques.Domain.Repository;
using EF.Estoques.Infra;
using EF.Estoques.Infra.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config.Estoques;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesEstoques(this IServiceCollection services, IConfiguration configuration)
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
}