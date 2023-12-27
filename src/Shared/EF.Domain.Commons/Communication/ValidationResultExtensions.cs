using System.Collections.ObjectModel;
using FluentValidation.Results;

namespace EF.Domain.Commons.Communication;

public static class ValidationResultExtensions
{
    public static ReadOnlyCollection<string> ToReadOnlyErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(e => e.ErrorMessage).ToList().AsReadOnly();
    }
}