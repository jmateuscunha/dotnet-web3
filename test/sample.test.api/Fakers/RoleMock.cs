using Bogus;
using model.api;

namespace sample.test.api.Fakers;

public static class RoleMock
{
    public static Faker<Role> ValidRoleFaker =>
            new Faker<Role>()
               .RuleFor(u => u.Id, f => f.Random.Int(1, 100))
               .RuleFor(u => u.Name, f => f.Company.CompanyName())
               .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
               .RuleFor(u => u.UpdatedAt, f => DateTime.UtcNow);
}