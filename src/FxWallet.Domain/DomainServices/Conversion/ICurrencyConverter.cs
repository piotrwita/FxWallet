using FxWallet.Domain.Shared;

namespace FxWallet.Domain.DomainServices.Conversion;

public interface ICurrencyConverter
{
    Task<Money> ConvertAsync(Money amount, Currency targetCurrency, CancellationToken cancellationToken = default);
}