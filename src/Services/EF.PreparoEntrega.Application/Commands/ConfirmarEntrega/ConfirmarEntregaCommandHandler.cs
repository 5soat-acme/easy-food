using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.PreparoEntrega.Domain.Repository;
using MediatR;

namespace EF.PreparoEntrega.Application.Commands.ConfirmarEntrega;

public class ConfirmarEntregaCommandHandler : CommandHandler,
    IRequestHandler<ConfirmarEntregaCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public ConfirmarEntregaCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(ConfirmarEntregaCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);

        pedido.ConfirmarEntrega();

        pedido.AddEvent(new EntregaRealizadaEvent
        {
            AggregateId = pedido.Id,
            CorrelacaoId = pedido.CorrelacaoId
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}