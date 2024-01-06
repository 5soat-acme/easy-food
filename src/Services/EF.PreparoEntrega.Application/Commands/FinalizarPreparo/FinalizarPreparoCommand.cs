using EF.Domain.Commons.Messages;

namespace EF.PreparoEntrega.Application.Commands.FinalizarPreparo;

public class FinalizarPreparoCommand : Command
{
    public Guid PedidoId { get; set; }
}