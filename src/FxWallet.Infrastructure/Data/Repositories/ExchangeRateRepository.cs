using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;
using FxWallet.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FxWallet.Infrastructure.Data.Repositories;

internal sealed class ExchangeRateRepository(FxWalletDbContext dbContext) : IExchangeRateRepository
{
    public async Task<ExchangeRate?> GetCurrentRateToPlnAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(currency);

        var exchangeRate = await dbContext.ExchangeRates
            .Where(e => e.FromCurrencyCode == currency.Code && e.ToCurrencyCode == Currency.PLN.Code)
            .OrderByDescending(e => e.EffectiveDate)
            .ThenByDescending(e => e.FetchedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (exchangeRate is null)
        {
            return null;
        }

        return ExchangeRate.CreateToPln(
            Currency.FromCode(exchangeRate.FromCurrencyCode),
            exchangeRate.Rate);
    }

    public async Task AddAsync(IEnumerable<ExchangeRate> rates, DateOnly effectiveDate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(rates);

        var exchangeRateDbModels = rates.Select(rate =>
        {
            return new ExchangeRateDbModel
            {
                Id = Guid.NewGuid(),
                FromCurrencyCode = rate.FromCurrency.Code,
                ToCurrencyCode = rate.ToCurrency.Code,
                Rate = rate.Rate,
                EffectiveDate = effectiveDate
            };
        });

        await dbContext.ExchangeRates.AddRangeAsync(exchangeRateDbModels, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
