using EF.Domain.Commons.Messages;
using MediatR;

namespace EF.Domain.Commons.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CommandResult> Send<T>(T command, CancellationToken cancellationToken = default) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task Publish<T>(T @event) where T : Event
    {
        await _mediator.Publish(@event);
    }
}