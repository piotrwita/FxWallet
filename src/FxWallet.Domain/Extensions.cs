using FxWallet.Domain.DomainServices.Conversion;
using FxWallet.Domain.DomainServices.Conversion.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace FxWallet.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IConversionPolicy, IdentityConversionPolicy>();
        services.AddScoped<IConversionPolicy, CrossRateConversionPolicy>();
        services.AddScoped<IConversionPolicy, FromPlnConversionPolicy>();
        services.AddScoped<IConversionPolicy, ToPlnConversionPolicy>();

        services.AddScoped<ICurrencyConverter, CurrencyConverter>();

        return services;
    }
}