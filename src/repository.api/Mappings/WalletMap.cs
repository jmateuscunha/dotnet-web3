using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model.api;

namespace repository.api;

public class WalletMap : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever().HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasColumnType("text").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();
        builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("bigint").IsRequired();
        builder.Property(x => x.BlockchainId).HasColumnName("blockchain_id").HasColumnType("uuid").IsRequired();

        builder.HasOne(w => w.Account).WithMany(a => a.Wallets).HasForeignKey(fk => fk.AccountId).IsRequired();
        builder.HasOne(w => w.Blockchain).WithMany(b => b.Wallets).HasForeignKey(fk => fk.BlockchainId).IsRequired();

        builder.ToTable("wallet");
    }
}

