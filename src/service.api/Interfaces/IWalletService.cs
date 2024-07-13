using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;

namespace service.api.Interfaces;

public interface IWalletService
{
    Task Add(AddWalletInputDto wallet);
    Task<AddAssetOutputDto> AddAsset(AddAssetInputDto dto);
    Task<IEnumerable<WalletOutputDto>> GetWalletsByAccount();
}