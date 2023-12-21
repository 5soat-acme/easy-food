using FluentValidation.Results;

namespace EF.Domain.Commons.Communication;

public class OperationResult<T> : OperationResult
{
    public T? Data { get; private set; }

    private OperationResult() { }

    public static new OperationResult<T> Success()
    {
        return new OperationResult<T> { IsValid = true, Errors = new List<string>().AsReadOnly() };
    }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T> { IsValid = true, Data = data, Errors = new List<string>().AsReadOnly() };
    }

    public static new OperationResult<T> Failure(string error)
    {
        return new OperationResult<T> { IsValid = false, Errors = new List<string> { error }.AsReadOnly() };
    }

    public static new OperationResult<T> Failure(List<string> error)
    {
        return new OperationResult<T> { IsValid = false, Errors = error.AsReadOnly() };
    }

    public static new OperationResult<T> Failure(ValidationResult validationResult)
    {
        return new OperationResult<T> { IsValid = false, Errors = validationResult.ToReadOnlyErrors() };
    }
}