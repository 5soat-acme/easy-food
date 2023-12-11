using FluentValidation.Results;

namespace EF.Domain.Commons.Messages;

public class CommandResult
{
    public ValidationResult ValidationResult { get; init; }
    public Guid AggregateId { get; init; }

    public bool IsValid()
    {
        return ValidationResult.IsValid;
    }

    public static CommandResult Create(ValidationResult validationResult, Guid aggregateId = default)
    {
        return new CommandResult { ValidationResult = validationResult, AggregateId = aggregateId };
    }
}