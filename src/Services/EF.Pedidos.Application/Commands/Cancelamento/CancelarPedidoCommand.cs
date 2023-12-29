using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.Cancelamento;

public class CancelarPedidoCommand : Command
{
    public Guid PedidoId { get; set; }
}