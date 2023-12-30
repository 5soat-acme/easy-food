using EF.Domain.Commons.Messages;
using EF.Pedidos.Application.Commands.CriarPedido;
using EF.Pedidos.Application.Mappings;
using EF.Pedidos.Application.Ports;
using EF.Pedidos.Application.Queries;
using EF.Pedidos.Application.Queries.Interfaces;
using EF.Pedidos.Domain.Repository;
using EF.Pedidos.Infra.Data;
using EF.Pedidos.Infra.Data.Repository;
using EF.Pedidos.Infra.Integrations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config.Pedidos;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServicesPedidos(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Application - Commands
        services
            .AddScoped<IRequestHandler<CriarPedidoCommand, CommandResult>, CriarPedidoCommandHandler>();

        // Application - Queries
        services.AddScoped<IPedidoQuery, PedidoQuery>();

        // Application - Ports
        services.AddScoped<IEstoqueService, EstoqueService>();

        // Application - Mapping
        services.AddAutoMapper(typeof(DomainToDtoProfile));

        // Infra - Data
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddDbContext<PedidoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}