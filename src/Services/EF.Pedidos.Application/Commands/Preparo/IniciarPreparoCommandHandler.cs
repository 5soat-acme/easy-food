using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.Preparo;

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
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

        pedido.IniciarPreparo();

        pedido.AddEvent(new PreparoPedidoIniciadoEvent
        {
            AggregateId = pedido.Id,
            PedidoId = pedido.Id
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}