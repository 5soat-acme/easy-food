using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages.Integrations;
using EF.PreparoEntrega.Application.Commands.CriarPedido;
using MediatR;

namespace EF.PreparoEntrega.Application.Services.Integrations;

public class PedidoIntegrationService : INotificationHandler<PagamentoProcessadoEvent>
{
    private readonly IMediatorHandler _mediator;

    public PedidoIntegrationService(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(PagamentoProcessadoEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CriarPedidoPreparoCommand
        {
            CorrelacaoId = notification.AggregateId,
            Itens = notification.Itens.Select(x => new CriarPedidoPreparoCommand.ItemPedido
            {
                ProdutoId = x.ProdutoId,
                Quantidade = x.Quantidade,
                NomeProduto = x.NomeProduto,
                TempoPreparoEstimado = x.TempoPreparoEstimado
            }).ToList()
        });
    }
}