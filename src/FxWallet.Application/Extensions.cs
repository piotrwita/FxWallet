using FxWallet.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FxWallet.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(s => s.FromExecutingAssembly()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(s => s.FromExecutingAssembly()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}