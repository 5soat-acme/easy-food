using System.Text.Json.Serialization;
using EF.Api.Apis.Carrinho.Config;
using EF.Api.Apis.Clientes.Config;
using EF.Api.Apis.Cupons.Config;
using EF.Api.Apis.Estoques.Config;
using EF.Api.Apis.Identidade.Config;
using EF.Api.Apis.Pagamentos.Config;
using EF.Api.Apis.Pedidos.Config;
using EF.Api.Apis.PreparoEntrega.Config;
using EF.Api.Apis.Produtos.Config;
using EF.Api.Commons.Extensions;

namespace EF.Api.Commons.Config;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerConfig();

        services.AddEventBusConfig();

        services.RegisterServicesIdentidade();
        services.RegisterServicesCarrinho(configuration);
        services.RegisterServicesPagamentos(configuration);
        services.RegisterServicesCupons(configuration);
        services.RegisterServicesEstoques(configuration);
        services.RegisterServicesClientes(configuration);
        services.RegisterServicesPedidos(configuration);
        services.RegisterServicesPreparoEntrega(configuration);
        services.RegisterServicesProdutos(configuration);
        services.AddIdentityConfig(configuration);

        return services;
    }

    public static WebApplication UseApiConfig(this WebApplication app)
    {
        app.UseSwaggerConfig();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseIdentityConfig();

        app.UseMiddleware<ExceptionMiddleware>();

        app.SubscribeEventHandlers();

        return app;
    }
}