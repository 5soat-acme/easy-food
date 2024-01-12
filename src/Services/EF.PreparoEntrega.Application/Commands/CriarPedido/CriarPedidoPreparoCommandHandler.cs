using EF.Domain.Commons.Messages;
using EF.PreparoEntrega.Domain.Models;
using EF.PreparoEntrega.Domain.Repository;
using MediatR;

namespace EF.PreparoEntrega.Application.Commands.CriarPedido;

public class CriarPedidoPreparoCommandHandler : CommandHandler,
    IRequestHandler<CriarPedidoPreparoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public CriarPedidoPreparoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(CriarPedidoPreparoCommand request,
        CancellationToken cancellationToken)
    {
        var pedido = MapearPedido(request);
        var proximoCodigo = await _pedidoRepository.ObterProximoCodigo();
        pedido.GerarCodigo(proximoCodigo);
        _pedidoRepository.Criar(pedido);
        var result = await PersistData(_pedidoRepository.UnitOfWork);
        return CommandResult.Create(result);
    }

    private Pedido MapearPedido(CriarPedidoPreparoCommand request)
    {
        var pedido = new Pedido(request.CorrelacaoId);

        foreach (var item in request.Itens)
            pedido.AdicionarItem(new Item(item.Quantidade, item.ProdutoId, item.NomeProduto,
                item.TempoPreparoEstimado));

        return pedido;
    }
}