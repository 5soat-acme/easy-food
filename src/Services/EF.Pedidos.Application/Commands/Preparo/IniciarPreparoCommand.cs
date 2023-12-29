using EF.Domain.Commons.Messages;

namespace EF.Pedidos.Application.Commands.Preparo;

public class IniciarPreparoCommand : Command
{
    public Guid PedidoId { get; set; }
}