using FxWallet.Api.ErrorHandling;
using FxWallet.Domain;
using FxWallet.Application;
using FxWallet.Infrastructure;
using FxWallet.Infrastructure.Data;

namespace FxWallet.Api;

internal static class Extensions
{
    internal static IServiceCollection AddLayers(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(configuration);

    internal static async Task<WebApplication> UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        await app.Services.EnsureDatabaseCreatedAsync();
        app.UseWalletsApi();

        return app;
    }
}