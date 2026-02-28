using FxWallet.Application.ExchangeRates;
using FxWallet.Infrastructure.ExchangeRates.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FxWallet.Infrastructure.ExchangeRates.BackgroundServices;

internal sealed class NbpExchangeRatesRefreshService(
    IServiceScopeFactory scopeFactory,
    IOptions<ExchangeRatesOptions> options,
    ILogger<NbpExchangeRatesRefreshService> logger) : BackgroundService
{
    private readonly ExchangeRatesOptions _options = options.Value;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting NBP Exchange Rates fetching service with configuration: Interval: {Interval}", _options.Interval);
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = scopeFactory.CreateScope();
                IExchangeRatesRefreshService refreshService = scope.ServiceProvider.GetRequiredService<IExchangeRatesRefreshService>();

                await refreshService.RefreshAsync(stoppingToken);

                await Task.Delay(_options.Interval, stoppingToken);
            }
        }
        catch (OperationCanceledException ex) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogWarning(ex, "NBP Exchange Rates fetching service stopped");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "NBP Exchange Rates fetching service failed");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
