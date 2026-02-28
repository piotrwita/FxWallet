using FxWallet.Domain.DomainServices.Conversion.Exceptions;
using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion.Policies;

internal sealed class CrossRateConversionPolicy(IExchangeRateRepository repository) : IConversionPolicy
{
    public bool CanBeApplied(Currency sourceCurrency, Currency targetCurrency) =>
        sourceCurrency.Code != Currency.PLN.Code && targetCurrency.Code != Currency.PLN.Code;

    public async Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default)
    {
        var rateFromSourceToPln = await repository.GetCurrentRateToPlnAsync(amount.Currency, cancellationToken);
        var rateFromPlnToTarget = await repository.GetCurrentRateToPlnAsync(targetCurrency, cancellationToken);

        if (rateFromSourceToPln is null)
        {
            throw new ExchangeRateNotFoundException(amount.Currency.Code);
        }

        if (rateFromPlnToTarget is null)
        {
            throw new ExchangeRateNotFoundException(targetCurrency.Code);
        }

        var plnAmount = rateFromSourceToPln.Convert(amount);
        var inverseRate = ExchangeRate.CreateInverse(rateFromPlnToTarget);
        return inverseRate.Convert(plnAmount);
    }
}
