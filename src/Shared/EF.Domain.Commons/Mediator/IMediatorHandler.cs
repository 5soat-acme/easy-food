using EF.Domain.Commons.Messages;

namespace EF.Domain.Commons.Mediator;

public interface IMediatorHandler
{
    Task<CommandResult> Send<T>(T command, CancellationToken cancellationToken = default) where T : Command;
    Task Publish<T>(T @event) where T : Event;
}