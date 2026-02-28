using FxWallet.Infrastructure.ExchangeRates.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FxWallet.Infrastructure.ExchangeRates;

internal sealed class NbpApiClient(
    IHttpClientFactory httpClientFactory,
    IOptions<ExchangeRatesOptions> options,
    ILogger<NbpApiClient> logger) : INbpApiClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ExchangeRatesOptions.HttpClientName);
    private readonly ExchangeRatesOptions _options = options.Value;

    public async Task<string> GetExchangeRatesXmlAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(_options.NbpApiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error while fetching exchange rates from NBP API");
            throw;
        }
    }
}
