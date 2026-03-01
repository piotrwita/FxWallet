using FxWallet.Domain.Wallets;
using FxWallet.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FxWallet.Infrastructure.Data.Configurations;

internal sealed class WalletConfiguration : IEntityTypeConfiguration<WalletDbModel>
{
    public void Configure(EntityTypeBuilder<WalletDbModel> builder)
    {
        builder.ToTable("Wallets");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(WalletName.MaxLength);

        builder.Property(w => w.BalancesJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt);
    }
}
