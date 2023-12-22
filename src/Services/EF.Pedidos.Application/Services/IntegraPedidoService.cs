using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages.Integrations.CarrinhoIntegracao;
using EF.Pedidos.Application.Commands;
using MediatR;

namespace EF.Pedidos.Application.Services;

public class IntegraPedidoService(IMediatorHandler mediator) : INotificationHandler<CarrinhoFechadoEvent>
{
    public async Task Handle(CarrinhoFechadoEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new GerarPedidoCommand
            {
                ClienteId = notification.ClienteId,
                ValorTotal = notification.ValorTotal,
                Desconto = notification.Desconto,
                ValorFinal = notification.ValorFinal,
                Itens = notification.Itens.Select(x => new GerarPedidoCommand.ItemPedido
                {
                    ProdutoId = x.ProdutoId,
                    Quantidade = x.Quantidade,
                    ValorUnitario = x.ValorUnitario
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