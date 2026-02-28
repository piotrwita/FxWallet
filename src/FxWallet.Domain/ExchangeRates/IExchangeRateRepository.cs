using FxWallet.Domain.Shared;

namespace FxWallet.Domain.ExchangeRates;

public interface IExchangeRateRepository
{
    Task<ExchangeRate?> GetCurrentRateToPlnAsync(Currency currency, CancellationToken cancellationToken = default);
    Task AddAsync(IEnumerable<ExchangeRate> rates, DateOnly effectiveDate, CancellationToken cancellationToken = default);
}