using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.PreparoEntrega.Domain.Repository;
using MediatR;

namespace EF.PreparoEntrega.Application.Commands.IniciarPreparo;

public class IniciarPreparoCommandHandler : CommandHandler,
    IRequestHandler<IniciarPreparoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public IniciarPreparoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(IniciarPreparoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);

        pedido.ConfirmarEntrega();

        pedido.AddEvent(new PreparoPedidoIniciadoEvent
        {
            AggregateId = pedido.Id,
            CorrelacaoId = pedido.CorrelacaoId
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}