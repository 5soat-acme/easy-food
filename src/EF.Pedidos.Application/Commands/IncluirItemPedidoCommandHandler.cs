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

    public async Task<ValidationResult> Handle(IncluirItemPedidoCommand request, CancellationToken cancellationToken)
    {
        // Faça a lógica de negócio aqui
        var pedido = await VerificarSePedidoClienteExiste(request.ClienteId);

        if (pedido is null)
        {
            pedido = await CriarPedidoRascunho(request);
            await _pedidoRepository.Criar(pedido);
        }

        // Comando persiste dados 
        return await PersistData(_pedidoRepository.UnitOfWork);
    }

    private async Task<Pedido?> VerificarSePedidoClienteExiste(Guid clienteId)
    {
        return null;
    }

    private async Task<Pedido> CriarPedidoRascunho(IncluirItemPedidoCommand command)
    {
        var pedido = new Pedido(command.ClienteId);
        pedido.AdicionarItem(new Item(pedido.Id, command.ProdutoId, command.Quantidade));

        return pedido;
    }
}