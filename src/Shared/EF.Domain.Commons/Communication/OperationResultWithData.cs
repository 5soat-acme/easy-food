using FluentValidation.Results;

namespace EF.Domain.Commons.Communication;

public class OperationResult<T> : OperationResult
{
    private OperationResult()
    {
    }

    public T? Data { get; private set; }

    public new static OperationResult<T> Success()
    {
        return new OperationResult<T> { IsValid = true, Errors = new List<string>().AsReadOnly() };
    }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T> { IsValid = true, Data = data, Errors = new List<string>().AsReadOnly() };
    }

    public new static OperationResult<T> Failure(string error)
    {
        return new OperationResult<T> { IsValid = false, Errors = new List<string> { error }.AsReadOnly() };
    }

    public new static OperationResult<T> Failure(List<string> error)
    {
        return new OperationResult<T> { IsValid = false, Errors = error.AsReadOnly() };
    }

    public new static OperationResult<T> Failure(ValidationResult validationResult)
    {
        return new OperationResult<T> { IsValid = false, Errors = validationResult.ToReadOnlyErrors() };
    }
}