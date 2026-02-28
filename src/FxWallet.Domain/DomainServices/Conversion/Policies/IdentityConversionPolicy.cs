using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion.Policies;

internal sealed class IdentityConversionPolicy : IConversionPolicy
{
    public bool CanBeApplied(Currency sourceCurrency, Currency targetCurrency) =>
        sourceCurrency.Code == targetCurrency.Code;

    public Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default) 
        => Task.FromResult(amount);
}
