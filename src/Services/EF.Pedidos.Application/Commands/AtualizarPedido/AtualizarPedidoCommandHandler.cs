using EF.Domain.Commons.Messages;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.AtualizarPedido;

public class AtualizarPedidoCommandHandler : CommandHandler,
    IRequestHandler<AtualizarPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public AtualizarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(AtualizarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);
        pedido.AtualizarStatus(request.Status);
        _pedidoRepository.Atualizar(pedido);
        var result = await PersistData(_pedidoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }
}