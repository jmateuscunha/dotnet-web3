using Bogus;
using shared.api.Dtos.Inputs;

namespace sample.test.api.Fakers;

public static class AddAssetInputDtoMock
{
    public static Faker<AddAssetInputDto> ValidAddAssetInputDtoFaker =>
        new Faker<AddAssetInputDto>()
            .RuleFor(u => u.WalletId, f => f.Random.Guid());

    public static Faker<AddAssetInputDto> ValidAddAssetInputRecordFaker =>
        new Faker<AddAssetInputDto>().CustomInstantiator(f => new AddAssetInputDto(f.Random.Guid(),f.Random.Guid()));
}