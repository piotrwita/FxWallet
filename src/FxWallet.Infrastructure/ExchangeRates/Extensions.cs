using FxWallet.Application.ExchangeRates;
using FxWallet.Infrastructure.ExchangeRates.BackgroundServices;
using FxWallet.Infrastructure.ExchangeRates.Mapping;
using FxWallet.Infrastructure.ExchangeRates.Options;
using FxWallet.Infrastructure.ExchangeRates.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FxWallet.Infrastructure.ExchangeRates;

internal static class Extensions
{
    public static IServiceCollection AddExchangeRates(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INbpApiClient, NbpApiClient>();
        services.AddScoped<IXmlDeserializer, NbpXmlDeserializer>();
        services.AddScoped<INbpResponseMapper, NbpResponseMapper>();
        services.AddScoped<IExchangeRatesRefreshService, ExchangeRatesRefreshService>();

        services.Configure<ExchangeRatesOptions>(
            configuration.GetSection(ExchangeRatesOptions.SectionName));

        services.AddHttpClient(ExchangeRatesOptions.HttpClientName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
        });

        services.AddHostedService<NbpExchangeRatesRefreshService>();

        return services;
    }
}