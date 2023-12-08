using EF.Domain.Commons.Messages;
using EF.Pedidos.Domain.Models;
using EF.Pedidos.Domain.Repository;
using FluentValidation.Results;
using MediatR;

namespace EF.Pedidos.Application.Commands;

public class IncluirItemPedidoCommandHandler : CommandHandler,
    IRequestHandler<IncluirItemPedidoCommand, ValidationResult>
{
    private readonly IPedidoRepository _pedidoRepository;

    public IncluirItemPedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<ValidationResult> Handle(IncluirItemPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = new Pedido(command.ClienteId);
        pedido.AdicionarItem(new Item(pedido.Id, command.ProdutoId, command.Quantidade));
        await _pedidoRepository.Criar(pedido);
        return await PersistData(_pedidoRepository.UnitOfWork);
    }
}