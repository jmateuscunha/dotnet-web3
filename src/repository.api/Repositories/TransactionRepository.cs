using Microsoft.EntityFrameworkCore;
using model.api;
using repository.api.Interfaces;

namespace repository.api.Repositories;

public sealed class TransactionRepository(SampleDbContext db) : ITransactionRepository
{
    private readonly SampleDbContext _db = db;

    public async Task Add(Transaction transaction)
    {
        await _db.Transactions.AddAsync(transaction);

        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAll()
        => await _db.Transactions.AsNoTrackingWithIdentityResolution().ToListAsync();
    
}
