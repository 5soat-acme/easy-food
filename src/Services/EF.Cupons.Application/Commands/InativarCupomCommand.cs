using EF.Domain.Commons.Messages;

namespace EF.Cupons.Application.Commands;

public class InativarCupomCommand : Command
{
    public Guid CupomId { get; set; }
}