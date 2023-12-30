using System.Text.Json.Serialization;
using EF.Api.Config.Carrinho;
using EF.Api.Config.Clientes;
using EF.Api.Config.Cupons;
using EF.Api.Config.Estoques;
using EF.Api.Config.Identidade;
using EF.Api.Config.Pagamentos;
using EF.Api.Config.Pedidos;
using EF.Api.Extensions;
using EF.Domain.Commons.Mediator;

namespace EF.Api.Config;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfig();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.RegisterServicesIdentidade();
        services.RegisterServicesCarrinho(configuration);
        services.RegisterServicesPagamentos(configuration);
        services.RegisterServicesCupons(configuration);
        services.RegisterServicesEstoques(configuration);
        services.RegisterServicesClientes(configuration);
        services.RegisterServicesPedidos(configuration);

        services.AddIdentityConfig(configuration);

        return services;
    }

    public static WebApplication UseApiConfig(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseSwaggerConfig();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseIdentityConfig();

        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}