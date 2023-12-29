using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.Cancelamento;

public class CancelarPedidoCommandHandler : CommandHandler,
    IRequestHandler<CancelarPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public CancelarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(CancelarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

        pedido.Cancelar();

        pedido.AddEvent(new PedidoCanceladoEvent
        {
            AggregateId = pedido.Id,
            PedidoId = pedido.Id
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}