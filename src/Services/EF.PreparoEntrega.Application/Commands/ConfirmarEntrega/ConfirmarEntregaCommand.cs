using EF.Domain.Commons.Messages;

namespace EF.PreparoEntrega.Application.Commands.ConfirmarEntrega;

public class ConfirmarEntregaCommand : Command
{
    public Guid PedidoId { get; set; }
}