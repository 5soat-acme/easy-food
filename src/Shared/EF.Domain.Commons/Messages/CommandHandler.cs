using EF.Domain.Commons.Repository;
using FluentValidation.Results;

namespace EF.Domain.Commons.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult = new();

    protected void AddError(string message)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
    }

    protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
    {
        if (!await unitOfWork.Commit()) AddError("Ocorreu um erro ao persistir os dados");
        return ValidationResult;
    }
}