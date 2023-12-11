using FluentValidation.Results;

namespace EF.Domain.Commons.Communication;

public class Result<T>
{
    public bool IsValid { get; private set; }
    public T? Data { get; private set; }
    public List<string> Errors { get; private set; }

    public static Result<T> Success()
    {
        return new Result<T> { IsValid = true };
    }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsValid = true, Data = data };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T> { IsValid = false, Errors = [error] };
    }

    public static Result<T> Failure(List<string> error)
    {
        return new Result<T> { IsValid = false, Errors = error };
    }

    public static Result<T> Failure(ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => e.ErrorMessage);
        return new Result<T> { IsValid = false, Errors = errors.ToList() };
    }
}