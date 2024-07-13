using Bogus;
using model.api;

namespace sample.test.api.Fakers;

public static class AccountMock
{
    public static Faker<Account> ValidAccountFaker =>
        new Faker<Account>()
            .RuleFor(p => p.Id, f => f.Random.Int())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Password, f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(u => u.UpdatedAt, f => DateTime.UtcNow);
}