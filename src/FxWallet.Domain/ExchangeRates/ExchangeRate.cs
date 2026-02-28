using FxWallet.Domain.ExchangeRates.Exceptions;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.ExchangeRates;

public sealed record ExchangeRate
{
    public const decimal MinRate = 0.0001m;
    public const decimal MaxRate = 1_000_000;
    public Currency FromCurrency { get; private init; }
    public Currency ToCurrency { get; private init; }
    public decimal Rate { get; private init; }

    private ExchangeRate(Currency fromCurrency, Currency toCurrency, decimal rate)
    {
        ArgumentNullException.ThrowIfNull(fromCurrency);

        ArgumentNullException.ThrowIfNull(toCurrency);

        if (fromCurrency.Code == toCurrency.Code)
        {
            throw new SameCurrenciesException(fromCurrency.Code);
        }

        if (rate is <= 0 or < MinRate or > MaxRate)
        {
            throw new InvalidExchangeRateValueException(rate);
        }

        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
    }

    public static ExchangeRate CreateToPln(Currency from, decimal rate)
    {
        if (from.Code == Currency.PLN.Code)
        {
            throw new CannotCreatePlnToPlnExchangeRateException();
        }

        return new ExchangeRate(from, Currency.PLN, rate);
    }

    public static ExchangeRate CreateFromPln(Currency to, decimal rate)
    {
        if (to.Code == Currency.PLN.Code)
        {
            throw new CannotCreatePlnToPlnExchangeRateException();
        }

        return new ExchangeRate(Currency.PLN, to, rate);
    }

    public static ExchangeRate CreateInverse(ExchangeRate exchangeRate)
    {
        ArgumentNullException.ThrowIfNull(exchangeRate);

        if (exchangeRate.ToCurrency.Code != Currency.PLN.Code)
        {
            throw new CannotCreateInverseExchangeRateException();
        }

        decimal inverseExchangeRate = 1 / exchangeRate.Rate;
        return new ExchangeRate(Currency.PLN, exchangeRate.FromCurrency, inverseExchangeRate);
    }

    internal Money Convert(Money amount)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Currency.Code != FromCurrency.Code)
        {
            throw new CurrencyMismatchException(FromCurrency.Code, amount.Currency.Code);
        }

        var convertedAmount = amount.Amount * Rate;
        return Money.Create(convertedAmount, ToCurrency);
    }
}