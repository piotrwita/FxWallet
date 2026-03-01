using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;
using FxWallet.Infrastructure.Data.Models;
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

        return MapToDomain(wallet);
    }

    public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<WalletDbModel> models = await dbContext.Wallets
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return models.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        WalletDbModel model = MapToDbModel(wallet);
        dbContext.Wallets.Add(model);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        WalletDbModel? existing = await dbContext.Wallets.FindAsync([wallet.Id.Value], cancellationToken);
        if (existing is null)
        {
            throw new WalletNotFoundException(wallet.Id.Value);
        }

        existing.Name = wallet.Name.Value;
        existing.BalancesJson = SerializeBalances(wallet.Balances);
        existing.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
