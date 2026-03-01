using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Shared;

public sealed record Money
{
    public decimal Amount { get; private init; }
    public Currency Currency { get; private init; }

    private Money(decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        if (amount < 0)
        {
            throw new NegativeAmountException(amount);
        }

        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency) => new(amount, currency);

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Currency.Code != other.Currency.Code)
        {
            throw new CurrencyMismatchException(Currency.Code, other.Currency.Code);
        }

        return Create(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Currency.Code != other.Currency.Code)
        {
            throw new CurrencyMismatchException(Currency.Code, other.Currency.Code);
        }

        decimal result = Amount - other.Amount;
        if (result < 0)
        {
            throw new NegativeResultException(Amount, other.Amount);
        }

        return Create(result, Currency);
    }
}
