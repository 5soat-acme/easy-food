using FluentValidation.Results;

namespace EF.Domain.Commons.Messages;

public class CommandResult
{
    public ValidationResult ValidationResult { get; private set; }
    public Guid AggregateId { get; private set; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }

    public static CommandResult Create(ValidationResult validationResult, Guid aggregateId = default)
    {
        return new CommandResult { ValidationResult = validationResult, AggregateId = aggregateId };
    }
}