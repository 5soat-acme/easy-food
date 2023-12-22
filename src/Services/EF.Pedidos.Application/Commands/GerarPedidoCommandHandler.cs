using EF.Domain.Commons.Messages;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands;

public class GerarPedidoCommandHandler : CommandHandler,
    IRequestHandler<GerarPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public GerarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(GerarPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = new Pedido(command.ClienteId);
        //pedido.AdicionarItem();
        await _pedidoRepository.Criar(pedido);
        var result = await PersistData(_pedidoRepository.UnitOfWork);
        return CommandResult.Create(result, pedido.Id);
    }
}