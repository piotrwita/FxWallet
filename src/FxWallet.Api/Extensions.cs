using FxWallet.Domain;
using FxWallet.Application;
using FxWallet.Infrastructure;

namespace FxWallet.Api;

internal static class Extensions
{
    internal static IServiceCollection AddLayers(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(configuration);
}