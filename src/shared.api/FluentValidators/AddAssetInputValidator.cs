using FluentValidation;
using shared.api.Dtos.Inputs;

namespace shared.api.FluentValidators;

public class AddAssetInputValidator : AbstractValidator<AddAssetInputDto>
{
    public AddAssetInputValidator()
    {
        RuleFor(f => f.WalletId).NotNull().NotEmpty().WithMessage("WalletId missing.");
    }
}
