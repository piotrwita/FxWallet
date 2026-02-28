using FxWallet.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FxWallet.Infrastructure.Data.Configurations;

internal sealed class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRateDbModel>
{
    public void Configure(EntityTypeBuilder<ExchangeRateDbModel> builder)
    {
        builder.ToTable("ExchangeRates");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FromCurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.ToCurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.Rate)
            .HasPrecision(18, 6)
            .IsRequired();

        builder.Property(e => e.EffectiveDate)
            .IsRequired();

        builder.Property(e => e.FetchedAt)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasIndex(e => new { e.FromCurrencyCode, e.EffectiveDate })
            .HasDatabaseName("IX_ExchangeRates_FromCurrency_EffectiveDate");

        builder.HasIndex(e => e.EffectiveDate)
            .HasDatabaseName("IX_ExchangeRates_EffectiveDate");

        builder.HasAlternateKey(e => new { e.FromCurrencyCode, e.EffectiveDate });
    }
}