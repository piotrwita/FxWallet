using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;
using FxWallet.Infrastructure.ExchangeRates.Dtos;
using Microsoft.Extensions.Logging;

namespace FxWallet.Infrastructure.ExchangeRates.Mapping;

internal sealed class NbpResponseMapper(ILogger<NbpResponseMapper> logger) : INbpResponseMapper
{
    public (IEnumerable<ExchangeRate> rates, DateOnly effectiveDate) Map(NbpExchangeRatesResponseDto responseDto)
    {
        if (responseDto.ExchangeRatesTable is null)
        {
            logger.LogWarning("ExchangeRatesTable is null in response DTO");
            return (new List<ExchangeRate>(), DateOnly.FromDateTime(default));
        }

        var exchangeRatesTable = responseDto.ExchangeRatesTable;
        var effectiveDate = DateOnly.FromDateTime(exchangeRatesTable.EffectiveDate);

        List<ExchangeRate> exchangeRates = [];
        foreach (NbpRateDto rateDto in exchangeRatesTable.Rates)
        {
            try
            {
                Currency currency = Currency.FromCode(rateDto.Code);
                ExchangeRate exchangeRate = ExchangeRate.CreateToPln(currency, rateDto.Mid);
                exchangeRates.Add(exchangeRate);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to create exchange rate for currency {CurrencyCode}", rateDto.Code);
            }
        }

        return (exchangeRates, effectiveDate);
    }
}
