using FxWallet.Domain.DomainServices.Conversion.Exceptions;
using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion.Policies;

internal sealed class ToPlnConversionPolicy(IExchangeRateRepository repository) : IConversionPolicy
{
    public bool CanBeApplied(Currency sourceCurrency, Currency targetCurrency) =>
        sourceCurrency.Code != Currency.PLN.Code && targetCurrency.Code == Currency.PLN.Code;

    public async Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default)
    {
        var rateToPln = await repository.GetCurrentRateToPlnAsync(amount.Currency, cancellationToken);
        if (rateToPln is null)
        {
            throw new ExchangeRateNotFoundException(amount.Currency.Code);
        }

        return rateToPln.Convert(amount);
    }
}
