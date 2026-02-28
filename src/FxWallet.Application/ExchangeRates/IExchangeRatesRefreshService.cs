namespace FxWallet.Application.ExchangeRates;

public interface IExchangeRatesRefreshService
{
    Task RefreshAsync(CancellationToken cancellationToken = default);
}