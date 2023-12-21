namespace EF.Domain.Commons.Communication;

using FluentValidation.Results;
using System.Collections.ObjectModel;

public class OperationResult
{
    public bool IsValid { get; protected set; }
    public ReadOnlyCollection<string> Errors { get; protected set; }

    protected OperationResult() { }

    public static OperationResult Success()
    {
        return new OperationResult { IsValid = true, Errors = new List<string>().AsReadOnly() };
    }

    public static OperationResult Failure(string error)
    {
        return new OperationResult { IsValid = false, Errors = new List<string> { error }.AsReadOnly() };
    }

    public static OperationResult Failure(List<string> error)
    {
        return new OperationResult { IsValid = false, Errors = error.AsReadOnly() };
    }

    public static OperationResult Failure(ValidationResult validationResult)
    {
        return new OperationResult { IsValid = false, Errors = validationResult.ToReadOnlyErrors() };
    }
}
