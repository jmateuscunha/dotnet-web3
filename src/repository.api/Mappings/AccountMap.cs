using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using model.api;

namespace repository.api;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd().HasColumnType("bigint");
        builder.Property(x => x.Email).HasColumnName("email").HasColumnType("text");
        builder.Property(x => x.Password).HasColumnName("password").HasColumnType("text").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
        builder.HasMany(r => r.Roles).WithMany(r => r.Accounts).UsingEntity("accounts_roles");

        builder.ToTable("account");
    }
}