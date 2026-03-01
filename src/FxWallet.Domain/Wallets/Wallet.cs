using FxWallet.Domain.DomainServices.Conversion;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Domain.Wallets;

public sealed class Wallet
{
    public WalletId Id { get; private init; } = null!;
    public WalletName Name { get; private set; } = null!;

    private readonly List<WalletBalance> _balances = [];
    
    public IReadOnlyList<WalletBalance> Balances => _balances.AsReadOnly();

    private Wallet() { }

    private Wallet(WalletId id, WalletName name, List<WalletBalance> balances)
    {
        Id = id;
        Name = name;
        _balances = balances;
    }

    public static Wallet Create(WalletId id, WalletName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);

        return new Wallet(id, name, []);
    }

    public void Rename(WalletName newName)
    {
        ArgumentNullException.ThrowIfNull(newName);

        Name = newName;
    }

    public void Deposit(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        var balance = EnsureBalance(amount.Currency);
        balance.Deposit(amount);
    }

    public void Withdraw(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        var balance = GetBalance(amount.Currency);
        if (balance is null)
        {
            throw new BalanceNotFoundException(Id.Value, amount.Currency.Code);
        }

        balance.Withdraw(amount);
        if (balance.Balance.Amount == 0)
        {
            _balances.Remove(balance);
        }
    }

    public async Task ExchangeAsync(
        Money fromAmount,
        Currency toCurrency,
        ICurrencyConverter converter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fromAmount);
        ArgumentNullException.ThrowIfNull(toCurrency);
        ArgumentNullException.ThrowIfNull(converter);

        if (fromAmount.Currency.Code == toCurrency.Code)
        {
            throw new CannotExchangeSameCurrencyException(fromAmount.Currency.Code);
        }

        var convertedAmount = await converter.ConvertAsync(fromAmount, toCurrency, cancellationToken);
        Withdraw(fromAmount);
        Deposit(convertedAmount);
    }

    private WalletBalance? GetBalance(Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        return _balances.SingleOrDefault(b => b.Balance.Currency.Code == currency.Code);
    }

    private WalletBalance EnsureBalance(Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        var balance = GetBalance(currency);
        if (balance is not null)
        {
            return balance;
        }

        var newBalance = WalletBalance.Create(Money.Create(0, currency));
        _balances.Add(newBalance);
        return newBalance;
    }
}