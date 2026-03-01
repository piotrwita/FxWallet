namespace FxWallet.Domain.Wallets;

public interface IWalletRepository
{
    Task<Wallet?> GetByIdAsync(WalletId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default);
}