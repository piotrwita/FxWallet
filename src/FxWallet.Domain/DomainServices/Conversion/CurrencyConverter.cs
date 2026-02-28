using FxWallet.Domain.DomainServices.Conversion.Exceptions;
using FxWallet.Domain.DomainServices.Conversion.Policies;
using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion;

internal sealed class CurrencyConverter(IEnumerable<IConversionPolicy> policies) : ICurrencyConverter
{
    public async Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(targetCurrency);

        var policy = policies.FirstOrDefault(p => p.CanBeApplied(amount.Currency, targetCurrency));
        if (policy is null)
        {
            throw new ConversionPolicyNotFoundException(amount.Currency.Code, targetCurrency.Code);
        }

        return await policy.ConvertAsync(amount, targetCurrency, cancellationToken);
    }
}
