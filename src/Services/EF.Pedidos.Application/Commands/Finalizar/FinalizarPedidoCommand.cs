using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.Finalizacao;

public class FinalizarPedidoCommand : Command
{
    public Guid PedidoId { get; set; }
}