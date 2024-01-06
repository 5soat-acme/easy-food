using EF.Domain.Commons.Messages;
using EF.Pedidos.Domain.Models;

namespace EF.Pedidos.Application.Commands.AtualizarPedido;

public class AtualizarPedidoCommand : Command
{
    public Guid PedidoId { get; set; }
    public Status Status { get; set; }
}