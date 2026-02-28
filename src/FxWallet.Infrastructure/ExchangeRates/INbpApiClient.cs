namespace FxWallet.Infrastructure.ExchangeRates;

internal interface INbpApiClient
{
    Task<string> GetExchangeRatesXmlAsync(CancellationToken cancellationToken = default);
}