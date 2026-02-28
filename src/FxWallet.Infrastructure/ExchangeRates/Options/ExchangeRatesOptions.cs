namespace FxWallet.Infrastructure.ExchangeRates.Options;

internal sealed record ExchangeRatesOptions
{
    public const string SectionName = "ExchangeRates";
    public const string HttpClientName = "NbpApi";

    public required string NbpApiUrl { get; init; } = string.Empty;
    public TimeSpan Interval { get; init; } = TimeSpan.FromHours(1);
}