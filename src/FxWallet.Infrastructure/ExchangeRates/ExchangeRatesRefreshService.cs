using FxWallet.Application.ExchangeRates;
using FxWallet.Domain.ExchangeRates;
using FxWallet.Infrastructure.ExchangeRates.Mapping;
using FxWallet.Infrastructure.ExchangeRates.Serialization;
using Microsoft.Extensions.Logging;

namespace FxWallet.Infrastructure.ExchangeRates;

internal sealed class ExchangeRatesRefreshService(
    INbpApiClient nbpApiClient,
    IXmlDeserializer xmlDeserializer,
    INbpResponseMapper responseMapper,
    IExchangeRateRepository repository,
    ILogger<ExchangeRatesRefreshService> logger) : IExchangeRatesRefreshService
{
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            string xmlContent = await nbpApiClient.GetExchangeRatesXmlAsync(cancellationToken);

            var responseDto = xmlDeserializer.Deserialize(xmlContent);
            if (responseDto is null || responseDto.ExchangeRatesTable is null)
            {
                logger.LogWarning("No exchange rates data received from NBP API");
                return;
            }

            (var exchangeRates, var effectiveDate) = responseMapper.Map(responseDto);

            if (exchangeRates.Any())
            {
                await repository.AddAsync(exchangeRates, effectiveDate, cancellationToken);
                logger.LogInformation("Successfully saved {Count} exchange rates for effective date {EffectiveDate}",
                    exchangeRates.Count(), effectiveDate);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching and saving exchange rates");
            throw;
        }
    }
}
