using Microsoft.EntityFrameworkCore;
using model.api;

namespace repository.api;

public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Blockchain> Blockchains { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(SampleDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.Entity<Role>().HasData(new Role { Id = -1, Name = "ADMIN", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        modelBuilder.Entity<Blockchain>().HasData(new Blockchain { Id = Guid.NewGuid(), Name = "Ethereum-Sepolia", Network = "Testnet", ChainId = 11155111, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.Now });

        base.OnModelCreating(modelBuilder);
    }
}
