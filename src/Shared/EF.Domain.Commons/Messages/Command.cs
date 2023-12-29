using MediatR;

namespace EF.Domain.Commons.Messages;

public abstract class Command : Message, IRequest<CommandResult>
{
}