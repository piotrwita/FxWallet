using FxWallet.Domain.Shared;

namespace FxWallet.Domain.Wallets;

public sealed class WalletBalance
{
    public Money Balance { get; private set; } = null!;

    private WalletBalance() { }

    private WalletBalance(Money balance)
    {
        Balance = balance;
    }

    public static WalletBalance Create(Money balance)
    {
        ArgumentNullException.ThrowIfNull(balance);

        return new WalletBalance(balance);
    }

    public void Deposit(Money amount)
    {
        Balance = Balance.Add(amount);
    }

    public void Withdraw(Money amount)
    {
        Balance = Balance.Subtract(amount);
    }
}