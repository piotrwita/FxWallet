using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;
using FxWallet.Infrastructure.Data.Models;
using FxWallet.Infrastructure.Data.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FxWallet.Infrastructure.Data.Repositories;

internal sealed class WalletRepository(FxWalletDbContext dbContext) : IWalletRepository
{
    public async Task<Wallet?> GetByIdAsync(WalletId id, CancellationToken cancellationToken = default)
    {
        var wallet = await dbContext.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id.Value, cancellationToken);

        if (wallet is null)
        {
            return null;
        }

        return WalletSerializer.Deserialize(wallet.WalletObject);
    }

    public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<WalletDbModel> models = await dbContext.Wallets
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return [.. models.Select(m => WalletSerializer.Deserialize(m.WalletObject))];
    }

    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        WalletDbModel model = new()
        {
            Id = wallet.Id.Value,
            WalletObject = WalletSerializer.Serialize(wallet),
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Wallets.Add(model);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Wallets.FindAsync([wallet.Id.Value], cancellationToken);
        if (existing is null)
        {
            throw new WalletNotFoundException(wallet.Id.Value);
        }

        existing.WalletObject = WalletSerializer.Serialize(wallet);
        existing.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}