using Bogus;
using shared.api.Dtos.Inputs;

namespace sample.test.api.Fakers;

public static class AccountDtoMock
{
    public static Faker<AccountInputDto> ValidAccountDtoFaker =>
            new Faker<AccountInputDto>()
               .RuleFor(u => u.Email, f => f.Company.CompanyName())
               .RuleFor(u => u.Password, f => f.Company.CompanyName())
               .RuleFor(u => u.Role, f => f.Company.CompanyName());
}