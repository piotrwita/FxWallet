using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion.Policies;

public interface IConversionPolicy
{
    bool CanBeApplied(Currency sourceCurrency, Currency targetCurrency);
    Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default);
}
