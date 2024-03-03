using EF.Pagamentos.Application.Config;
using EF.Pagamentos.Application.UseCases;
using EF.Pagamentos.Application.UseCases.Interfaces;
using EF.Pagamentos.Domain.Repository;
using EF.Pagamentos.Infra;
using EF.Pagamentos.Infra.Data.Repository;
using EF.Pagamentos.Infra.Services;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Contexts.Pagamentos.Config;

public static class DependencyInjectionConfig
{
    public static void RegisterServicesPagamentos(this IServiceCollection services, IConfiguration configuration)
    {
        // Application - Config
        services.AddScoped<PagamentoServiceResolver>();

        // Application - Use Cases
        services.AddScoped<IProcessarPagamentoUseCase, ProcessarPagamentoUseCase>();
        services.AddScoped<IAutorizarPagamentoUseCase, AutorizarPagamentoUseCase>();
        services.AddScoped<IConsultarPagamentoPedidoUseCase, ConsultarPagamentoPedidoUseCase>();


        // Domain
        services.AddScoped<PagamentoMercadoPagoService>();

        // Infra - Data 
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddDbContext<PagamentoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}