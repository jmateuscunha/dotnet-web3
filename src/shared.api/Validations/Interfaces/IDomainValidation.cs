using FluentValidation.Results;

namespace shared.api.Validations.Interfaces;

public interface IDomainValidation
{
    void AddErrors(IEnumerable<ValidationFailure> errors);
    void AddError(string errorMessage);
    bool HasErrors();
    IEnumerable<string> GetErrors();
    void Clear();
}
