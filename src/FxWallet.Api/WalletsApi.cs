using FxWallet.Api.Requests;
using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Commands;
using FxWallet.Application.Wallets.Dtos;
using FxWallet.Application.Wallets.Queries;

namespace FxWallet.Api;

internal static class WalletsApi
{
    public static WebApplication UseWalletsApi(this WebApplication app)
    {
        app.MapPost("api/v1/wallets", async (CreateWallet command, ICommandHandler<CreateWallet> handler, CancellationToken cancellationToken) =>
        {
            command = command with { WalletId = Guid.NewGuid() };
            await handler.HandleAsync(command, cancellationToken);
            return Results.Created($"/api/v1/wallets/{command.WalletId}", null);
        });

        app.MapPatch("api/v1/wallets/{id:guid}", async (Guid id, RenameWallet command, ICommandHandler<RenameWallet> handler, CancellationToken cancellationToken) =>
        {
            command = command with { WalletId = id };
            await handler.HandleAsync(command, cancellationToken);
            return Results.NoContent();
        });

        app.MapPost("api/v1/wallets/{id:guid}/deposit", async (Guid id, Deposit command, ICommandHandler<Deposit> handler, CancellationToken cancellationToken) =>
        {
            command = command with { WalletId = id };
            await handler.HandleAsync(command, cancellationToken);
            return Results.NoContent();
        });

        app.MapPost("api/v1/wallets/{id:guid}/withdraw", async (Guid id, Withdraw command, ICommandHandler<Withdraw> handler, CancellationToken cancellationToken) =>
        {
            command = command with { WalletId = id };
            await handler.HandleAsync(command, cancellationToken);
            return Results.NoContent();
        });

        app.MapPost("api/v1/wallets/{id:guid}/exchange", async (Guid id, Exchange command, ICommandHandler<Exchange> handler, CancellationToken cancellationToken) =>
        {
            command = command with { WalletId = id };
            await handler.HandleAsync(command, cancellationToken);
            return Results.NoContent();
        });

        app.MapGet("api/v1/wallets/{id:guid}", async (Guid id, IQueryHandler<GetWallet, WalletDto?> handler, CancellationToken cancellationToken) =>
        {
            var wallet = await handler.HandleAsync(new GetWallet(id), cancellationToken);
            return wallet is null ? Results.NotFound() : Results.Ok(wallet);
        })
        .WithName("GetWallet");

        app.MapGet("api/v1/wallets", async (IQueryHandler<GetAllWallets, IReadOnlyList<WalletDto>> handler, CancellationToken cancellationToken) =>
        {
            var wallets = await handler.HandleAsync(new GetAllWallets(), cancellationToken);
            return Results.Ok(wallets);
        });

        return app;
    }
}