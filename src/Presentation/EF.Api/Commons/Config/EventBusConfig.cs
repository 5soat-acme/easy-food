using EF.Carrinho.Application.Events;
using EF.Core.Commons.Messages;
using EF.Core.Commons.Messages.Integrations;
using EF.Infra.Commons.EventBus;
using EF.Pedidos.Application.Events;
using EF.PreparoEntrega.Application.Events;

namespace EF.Api.Commons.Config;

public static class EventBusConfig
{
    public static IServiceCollection AddEventBusConfig(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        
        // Carrinho
        services.AddScoped<IEventHandler<PedidoCriadoEvent>, CarrinhoEventHandler>();
        
        // Preparo entrega
        services.AddScoped<IEventHandler<PedidoRecebidoEvent>, PedidoEntregaEventHandler>();
        services.AddScoped<IEventHandler<PreparoPedidoIniciadoEvent>, PedidoEventHandler>();
        services.AddScoped<IEventHandler<PreparoPedidoFinalizadoEvent>, PedidoEventHandler>();
        services.AddScoped<IEventHandler<EntregaRealizadaEvent>, PedidoEventHandler>();
        
        // Pagamentos
        services.AddScoped<IEventHandler<PagamentoAutorizadoEvent>, PedidoEventHandler>();

        return services;
    }

    public static WebApplication SubscribeEventHandlers(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var bus = services.GetRequiredService<IEventBus>();
        
        services.GetRequiredService<IEnumerable<IEventHandler<PedidoCriadoEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));

        services.GetRequiredService<IEnumerable<IEventHandler<PedidoRecebidoEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));

        services.GetRequiredService<IEnumerable<IEventHandler<PreparoPedidoIniciadoEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));

        services.GetRequiredService<IEnumerable<IEventHandler<PreparoPedidoFinalizadoEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));

        services.GetRequiredService<IEnumerable<IEventHandler<EntregaRealizadaEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));
        
        services.GetRequiredService<IEnumerable<IEventHandler<PagamentoAutorizadoEvent>>>().ToList()
            .ForEach(e => bus.Subscribe(e));

        return app;
    }
}