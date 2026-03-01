namespace FxWallet.Domain.Wallets;

public sealed record WalletId(Guid Value)
{
    public static implicit operator Guid(WalletId id) => id.Value;
    public static implicit operator WalletId(Guid id) =>
        id.Equals(Guid.Empty) ? null! : new WalletId(id);
}