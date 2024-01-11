using System.ComponentModel.DataAnnotations;
using EF.Domain.Commons.Messages;

namespace EF.PreparoEntrega.Application.Commands.FinalizarPreparo;

public class FinalizarPreparoCommand : Command
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public Guid PedidoId { get; set; }
}