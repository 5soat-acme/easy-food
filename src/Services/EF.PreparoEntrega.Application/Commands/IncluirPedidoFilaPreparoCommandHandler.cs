using EF.Domain.Commons.Messages;
using EF.PreparoEntrega.Domain.Models;
using EF.PreparoEntrega.Domain.Repository;
using MediatR;

namespace EF.PreparoEntrega.Application.Commands;

public class IncluirPedidoFilaPreparoCommandHandler : CommandHandler,
    IRequestHandler<IncluirPedidoFilaPreparoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public IncluirPedidoFilaPreparoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(IncluirPedidoFilaPreparoCommand request,
        CancellationToken cancellationToken)
    {
        var pedido = MapearPedido(request);
        _pedidoRepository.Criar(pedido);
        var result = await PersistData(_pedidoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }

    private Pedido MapearPedido(IncluirPedidoFilaPreparoCommand request)
    {
        var pedido = new Pedido(request.CorrelacaoId);

        foreach (var item in request.Itens)
        {
            pedido.AdicionarItem(new Item(item.Quantidade, item.ProdutoId, item.NomeProduto,
                item.TempoPreparoEstimado));
        }

        return pedido;
    }
}