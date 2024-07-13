using Microsoft.EntityFrameworkCore;
using model.api;
using repository.api.Interfaces;
using shared.api.Exceptions;

namespace repository.api.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly SampleDbContext _db;
    public AccountRepository(SampleDbContext db)
    {
        _db = db;
    }
    public async Task Add(Account account)
    {
        var tranck = await _db.AddAsync(account);

        var result = await _db.SaveChangesAsync();
    }

    public async Task<Account> GetById(int id)
    {
        var result = await _db.Accounts.AsNoTrackingWithIdentityResolution()
                                       .Include(c => c.Roles)
                                       .FirstOrDefaultAsync(c => c.Id == id);

        return result is null ? throw new DomainException("SAMPLE-001", "Account not found.") : result;
    }

    public async Task<Account> GetByEmail(string email)
        => await _db.Accounts.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Email == email);

}
