using EF.Domain.Commons.Messages;
using FluentValidation.Results;

namespace EF.Domain.Commons.Mediator;

public interface IMediatorHandler
{
    Task<ValidationResult> Send<T>(T command) where T : Command;
    Task Publish<T>(T @event) where T : Event;
}