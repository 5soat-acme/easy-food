using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands;

public class CriarPedidoCommandHandler : CommandHandler,
    IRequestHandler<CriarPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public CriarPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(CriarPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = new Pedido(command.ClienteId, command.ValorTotal);

        foreach (var item in command.Itens)
            pedido.AdicionarItem(new Item(item.ProdutoId, item.NomeProduto, item.ValorUnitario, item.Quantidade,
                item.Desconto));

        pedido.AssociarCorrelacao(command.CorrelacaoId);

        _pedidoRepository.Criar(pedido);

        pedido.AddEvent(new PedidoCriadoEvent());
        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result, pedido.Id);
    }
}