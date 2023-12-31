using EF.Domain.Commons.Messages;

namespace EF.PreparoEntrega.Application.Commands.IniciarPreparo;

public class IniciarPreparoCommand : Command
{
    public Guid PedidoId { get; set; }
}