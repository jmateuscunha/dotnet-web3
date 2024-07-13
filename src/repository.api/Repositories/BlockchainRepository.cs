using Microsoft.EntityFrameworkCore;
using model.api;
using repository.api.Interfaces;
using shared.api.Exceptions;

namespace repository.api.Repositories;

public sealed class BlockchainRepository : IBlockchainRepository
{
    private readonly SampleDbContext _db;
    public BlockchainRepository(SampleDbContext db)
    {
        _db = db;
    }
    public async Task<Blockchain> GetBlockchainById(Guid id)
    {
        var result = await _db.Blockchains.SingleOrDefaultAsync(c => c.Id == id);

        return result is null ? throw new DomainException("SAMPLE-001", "Blockchain not found.") : result;
    }
}
