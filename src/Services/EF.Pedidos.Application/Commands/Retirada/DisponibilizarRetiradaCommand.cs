using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.Retirada;

public class DisponibilizarRetiradaCommand : Command
{
    public Guid PedidoId { get; set; }
}