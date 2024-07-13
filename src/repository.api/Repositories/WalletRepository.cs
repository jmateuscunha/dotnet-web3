using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using model.api;
using repository.api.Interfaces;
using shared.api.Exceptions;

namespace repository.api.Repositories;

public sealed class WalletRepository : IWalletRepository
{
    private readonly SampleDbContext _db;
    private readonly ILogger<WalletRepository> _logger;
    public WalletRepository(SampleDbContext db, ILogger<WalletRepository> logger)
    {
        _db = db;
        _logger=logger;
    }
    public async Task Add(Wallet wallet)
    {
        await _db.Wallets.AddAsync(wallet);

        await _db.SaveChangesAsync();
    }

    public async Task Update(Wallet wallet)
    {
        _logger.LogInformation(_db.ChangeTracker.DebugView.LongView);

        await _db.SaveChangesAsync();
    }

    public async Task AddAsset(Asset asset)
    {
        await _db.Assets.AddAsync(asset);

        await _db.SaveChangesAsync();
    }

    public async Task<Wallet> GetWalletById(Guid id)
    {
        var wallet = await _db.Wallets.Include(c => c.Assets).FirstOrDefaultAsync(c => c.Id == id);

        return wallet is null ? throw new DomainException("SAMPLE-001", "Wallet not found.") : wallet;
    }

    public async Task<Asset> GetAssetById(Guid id)
    {
        var asset = await _db.Assets.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Id == id);

        return asset is null ? throw new DomainException("SAMPLE-001","Asset not found.") : asset;
    }

    public async Task<IEnumerable<Wallet>> GetWalletsByAccount(int accountId, bool includeAssets = false)
    {
        var q = _db.Wallets.AsNoTrackingWithIdentityResolution();

        if (includeAssets) q = q.Include(a => a.Assets);

        var wallets = await q.Where(c => c.AccountId == accountId)
                             .ToListAsync();

        if (wallets is null || wallets.Count == 0) throw new DomainException("SAMPLE-001", "Wallets not found.");

        return wallets;
    }
}