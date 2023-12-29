using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Application.Commands.Recebimento;
using MediatR;

namespace EF.Pedidos.Application.Services;

public class IntegrarPedidoService(IMediatorHandler mediator) : INotificationHandler<CheckoutIniciadoEvent>
{
    public async Task Handle(CheckoutIniciadoEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new ReceberPedidoCommand
            {
                ClienteId = notification.ClienteId,
                CorrelacaoId = notification.AggregateId,
                ValorTotal = notification.ValorTotal,
                ValorFinal = notification.ValorFinal,
                Itens = notification.Itens.Select(i => new ReceberPedidoCommand.ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    ValorUnitario = i.ValorUnitario,
                    Desconto = i.Desconto,
                    ValorFinal = i.ValorFinal,
                    Quantidade = i.Quantidade,
                    TempoPreparoEstimado = i.TempoPreparoEstimado
                }).ToList()
            });
        }
        catch (Exception e)
        {
            // TODO: Transações de compensação
            Console.WriteLine(e);
            throw;
        }
    }
}