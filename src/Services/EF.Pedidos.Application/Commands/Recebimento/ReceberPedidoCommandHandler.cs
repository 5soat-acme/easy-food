using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Messages.Integrations;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using MediatR;

namespace EF.Pedidos.Application.Commands.Recebimento;

public class ReceberPedidoCommandHandler : CommandHandler,
    IRequestHandler<ReceberPedidoCommand, CommandResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public ReceberPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CommandResult> Handle(ReceberPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = MapearPedido(request);

        pedido.CalcularValorTotal();

        if (!ValidarPedido(pedido, request)) return CommandResult.Create(ValidationResult);

        _pedidoRepository.Criar(pedido);

        pedido.AddEvent(new PedidoCriadoEvent
        {
            AggregateId = pedido.Id,
            PedidoId = pedido.Id
        });

        var result = await PersistData(_pedidoRepository.UnitOfWork);

        return CommandResult.Create(result);
    }

    private Pedido MapearPedido(ReceberPedidoCommand request)
    {
        var pedido = new Pedido(request.ClienteId, request.Cpf);

        foreach (var item in request.Itens)
            pedido.AdicionarItem(new Item(item.ProdutoId, item.NomeProduto, item.ValorUnitario, item.Quantidade,
                item.Desconto));

        pedido.AssociarCorrelacao(request.CorrelacaoId);

        return pedido;
    }

    private bool ValidarPedido(Pedido pedido, ReceberPedidoCommand request)
    {
        if (pedido.ValorTotal != request.ValorFinal)
            AddError("O valor total do pedido não confere com o valor informado");

        return ValidationResult.IsValid;
    }
}