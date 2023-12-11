using EF.Domain.Commons.Messages;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands;

public class IncluirItemPedidoCommandHandler : CommandHandler,
    IRequestHandler<IncluirItemPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public IncluirItemPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(IncluirItemPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = new Pedido(command.ClienteId);
        pedido.AdicionarItem(new Item(pedido.Id, command.ProdutoId, command.Quantidade));
        await _pedidoRepository.Criar(pedido);
        var result = await PersistData(_pedidoRepository.UnitOfWork);
        return CommandResult.Create(result, pedido.Id);
    }
}