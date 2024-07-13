using model.api;

namespace repository.api.Interfaces;

public interface IWalletRepository
{
    Task Add(Wallet wallet);
    Task Update(Wallet wallet);
    Task<Wallet> GetWalletById(Guid id);
    Task<Asset> GetAssetById(Guid id);
    Task AddAsset(Asset asset);
    Task<IEnumerable<Wallet>> GetWalletsByAccount(int accountId, bool includeAssets = default);
}