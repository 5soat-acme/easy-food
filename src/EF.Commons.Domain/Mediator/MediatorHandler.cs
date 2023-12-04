using EF.Domain.Commons.Messages;
using FluentValidation.Results;
using MediatR;

namespace EF.Domain.Commons.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ValidationResult> Send<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task Publish<T>(T @event) where T : Event
    {
        await _mediator.Publish(@event);
    }
}