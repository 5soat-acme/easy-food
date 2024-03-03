using System.Text.Json.Serialization;
using EF.Api.Commons.Extensions;
using EF.Api.Contexts.Carrinho.Config;
using EF.Api.Contexts.Clientes.Config;
using EF.Api.Contexts.Cupons.Config;
using EF.Api.Contexts.Estoques.Config;
using EF.Api.Contexts.Identidade.Config;
using EF.Api.Contexts.Pagamentos.Config;
using EF.Api.Contexts.Pedidos.Config;
using EF.Api.Contexts.PreparoEntrega.Config;
using EF.Api.Contexts.Produtos.Config;

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
        
        services.Configure<PagamentoAutorizacaoWebHookSettings>(configuration.GetSection("PagamentoAutorizacaoWebHook"));

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