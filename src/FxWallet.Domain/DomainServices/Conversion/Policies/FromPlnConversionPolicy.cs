using FxWallet.Domain.DomainServices.Conversion.Exceptions;
using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion.Policies;

internal sealed class FromPlnConversionPolicy(IExchangeRateRepository repository) : IConversionPolicy
{
    public bool CanBeApplied(Currency sourceCurrency, Currency targetCurrency) =>
        sourceCurrency.Code == Currency.PLN.Code && targetCurrency.Code != Currency.PLN.Code;

    public async Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default)
    {
        var rateToTarget = await repository.GetCurrentRateToPlnAsync(targetCurrency, cancellationToken);
        if (rateToTarget is null)
        {
            throw new ExchangeRateNotFoundException(targetCurrency.Code);
        }

        var inverseRate = ExchangeRate.CreateInverse(rateToTarget);
        return inverseRate.Convert(amount);
    }
}
