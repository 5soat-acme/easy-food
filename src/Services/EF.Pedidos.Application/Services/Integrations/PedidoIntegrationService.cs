using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Application.Commands.AtualizarPedido;
using EF.Pedidos.Domain.Models;
using MediatR;

namespace EF.Pedidos.Application.Services.Integrations;

public class PedidoIntegrationService : INotificationHandler<PreparoPedidoIniciadoEvent>,
    INotificationHandler<PreparoPedidoFinalizadoEvent>,
    INotificationHandler<EntregaRealizadaEvent>
{
    private readonly IMediatorHandler _mediator;

    public PedidoIntegrationService(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(EntregaRealizadaEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AtualizarPedidoCommand
        {
            AggregateId = notification.AggregateId,
            PedidoId = notification.CorrelacaoId,
            Status = Status.Finalizado
        });
    }

    public async Task Handle(PreparoPedidoFinalizadoEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AtualizarPedidoCommand
        {
            AggregateId = notification.AggregateId,
            PedidoId = notification.CorrelacaoId,
            Status = Status.Pronto
        });
    }

    public async Task Handle(PreparoPedidoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AtualizarPedidoCommand
        {
            AggregateId = notification.AggregateId,
            PedidoId = notification.CorrelacaoId,
            Status = Status.EmPreparacao
        });
    }
}