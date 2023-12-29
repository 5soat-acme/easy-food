using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Application.Commands.Preparo;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.Retirada;

public class DisponibilizarRetiradaCommandHandler : CommandHandler,
    IRequestHandler<IniciarPreparoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public DisponibilizarRetiradaCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(IniciarPreparoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

        pedido.DisponibilizarRetirada();

        pedido.AddEvent(new PedidoProntoParaRetiradaEvent
        {
            AggregateId = pedido.Id,
            PedidoId = pedido.Id
        });

        _pedidoRepository.Atualizar(pedido);

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }
}