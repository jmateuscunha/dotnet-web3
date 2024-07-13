using Bogus;
using model.api;

namespace sample.test.api.Fakers;

public static class WalletMock
{
    public static Faker<Wallet> ValidWalletFaker =>
    new Faker<Wallet>()
    .RuleFor(p => p.Id, f => f.Random.Guid())
    .RuleFor(p => p.Name, f => f.Random.String());
}
