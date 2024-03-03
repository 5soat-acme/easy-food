using EF.Core.Commons.Messages;
using EF.Core.Commons.Messages.Integrations;
using EF.PreparoEntrega.Application.DTOs.Requests;
using EF.PreparoEntrega.Application.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EF.PreparoEntrega.Application.Events;

public class PedidoEntregaEventHandler : IEventHandler<PedidoRecebidoEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PedidoEntregaEventHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Handle(PedidoRecebidoEvent @event)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var criarPedidoUseCase = scope.ServiceProvider.GetRequiredService<ICriarPedidoUseCase>();

        await criarPedidoUseCase.Handle(new CriarPedidoPreparoDto
        {
            CorrelacaoId = @event.AggregateId,
            Itens = @event.Itens.Select(x => new CriarPedidoPreparoDto.ItemPedido
            {
                ProdutoId = x.ProdutoId,
                Quantidade = x.Quantidade,
                NomeProduto = x.NomeProduto,
                TempoPreparoEstimado = x.TempoPreparoEstimado
            }).ToList()
        });
    }
}