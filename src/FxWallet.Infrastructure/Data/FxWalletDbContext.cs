using FxWallet.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FxWallet.Infrastructure.Data;

internal sealed class FxWalletDbContext(DbContextOptions<FxWalletDbContext> options) : DbContext(options)
{
    public DbSet<ExchangeRateDbModel> ExchangeRates { get; set; } = null!;
    public DbSet<WalletDbModel> Wallets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FxWalletDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}