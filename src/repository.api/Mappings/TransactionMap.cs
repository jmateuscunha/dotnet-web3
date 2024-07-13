using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model.api;

namespace repository.api;

public class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Hash).HasColumnName("hash").HasColumnType("text").IsRequired();
        builder.Property(x => x.FromAddress).HasColumnName("from_address").HasColumnType("text").IsRequired();
        builder.Property(x => x.ToAddress).HasColumnName("to_address").HasColumnType("text").IsRequired();
        builder.Property(x => x.AmountRequested).HasColumnName("amount_requested").HasColumnType("numeric(1000)").IsRequired();
        builder.Property(x => x.AmountDeducted).HasColumnName("amount_deducted").HasColumnType("numeric(1000)").IsRequired();
        builder.Property(x => x.FeeApplied).HasColumnName("fee_applied").HasColumnType("numeric(1000)").IsRequired();
        builder.Property(x => x.AssetId).HasColumnName("asset_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Nonce).HasColumnName("nonce").HasColumnType("numeric(1000)").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasColumnType("text").HasDefaultValue("PENDING").IsRequired();
        builder.Property(x => x.BlockNumber).HasColumnName("block_number").HasColumnType("numeric(1000)").IsRequired(false);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();

        builder.HasOne(w => w.Asset).WithMany(a => a.Transactions).HasForeignKey(fk => fk.AssetId).IsRequired();

        builder.ToTable("transaction");
    }
}

