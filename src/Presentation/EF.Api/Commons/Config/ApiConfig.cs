using System.Text.Json.Serialization;
using EF.Api.Apis.Carrinho.Config;
using EF.Api.Apis.Clientes.Config;
using EF.Api.Apis.Cupons.Config;
using EF.Api.Apis.Estoques.Config;
using EF.Api.Apis.Identidade.Config;
using EF.Api.Apis.Pagamentos.Config;
using EF.Api.Apis.Pedidos.Config;
using EF.Api.Commons.Extensions;
using EF.Domain.Commons.Mediator;

namespace EF.Api.Commons.Config;

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