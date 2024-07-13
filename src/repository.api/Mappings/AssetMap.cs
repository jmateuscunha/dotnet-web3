using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model.api;

namespace repository.api;

public class AssetMap : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever().HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Address).HasColumnName("address").HasColumnType("text").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();
        builder.Property(x => x.Balance).HasColumnName("balance").HasColumnType("numeric(1000)").IsRequired();
        builder.Property(x => x.WalletId).HasColumnName("wallet_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Keystore).HasColumnName("keystore").HasColumnType("bytea").HasConversion<byte[]>().IsRequired();

        builder.HasOne(w => w.Wallet).WithMany(a => a.Assets).HasForeignKey(fk => fk.WalletId).IsRequired();

        builder.ToTable("asset");
    }
}