using EF.Domain.Commons.DomainObjects;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.PreparoEntrega.Domain.Repository;
using MediatR;

namespace EF.PreparoEntrega.Application.Commands.FinalizarPreparo;

public class FinalizarPreparoCommandHandler : CommandHandler,
    IRequestHandler<FinalizarPreparoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public FinalizarPreparoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(FinalizarPreparoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorId(request.PedidoId);
        if (pedido is null) throw new DomainException("Pedido inválido");

        pedido.FinalizarPreparo();

        pedido.AddEvent(new PreparoPedidoFinalizadoEvent
        {
            AggregateId = pedido.Id,
            PedidoCorrelacaoId = pedido.PedidoCorrelacaoId
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}