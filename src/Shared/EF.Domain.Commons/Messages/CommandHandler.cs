using EF.Domain.Commons.Repository;
using FluentValidation.Results;

namespace EF.Domain.Commons.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult = new();

    protected void AddError(string message, string propertyName = "")
    {
        ValidationResult.Errors.Add(new ValidationFailure(propertyName, message));
    }

    protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
    {
        if (!await unitOfWork.Commit()) AddError("Ocorreu um erro ao persistir os dados");
        return ValidationResult;
    }
}