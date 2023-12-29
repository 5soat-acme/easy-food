using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.Finalizacao;

public class FinalizarPedidoCommandHandler : CommandHandler,
    IRequestHandler<FinalizarPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public FinalizarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(FinalizarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

        pedido.Finalizar();

        pedido.AddEvent(new PedidoFinalizadoEvent
        {
            AggregateId = pedido.Id,
            PedidoId = pedido.Id
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}