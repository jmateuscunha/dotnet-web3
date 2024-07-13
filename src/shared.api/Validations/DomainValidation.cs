using FluentValidation.Results;
using shared.api.Validations.Interfaces;

namespace shared.api.Validations;

public class DomainValidation : IDomainValidation
{
    private readonly ValidationResult _validationResult;

    public DomainValidation()
    {
        _validationResult = new ValidationResult();
    }

    public void AddError(string errorMessage) => _validationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));
    public void AddErrors(IEnumerable<ValidationFailure> errors) => _validationResult.Errors.AddRange(errors);
    public IEnumerable<string> GetErrors() => _validationResult.Errors.Select(x => x.ErrorMessage);

    public bool HasErrors() => _validationResult.Errors.Count != 0;

    public void Clear() => _validationResult.Errors.Clear();
}
