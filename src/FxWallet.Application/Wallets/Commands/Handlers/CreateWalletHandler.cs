using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Commands;
using FxWallet.Domain.Wallets;

namespace FxWallet.Application.Wallets.Commands.Handlers;

internal sealed class CreateWalletHandler(IWalletRepository walletRepository) : ICommandHandler<CreateWallet>
{
    public async Task HandleAsync(CreateWallet command, CancellationToken cancellationToken = default)
    {
        var name = new WalletName(command.Name);
        var wallet = Wallet.Create(command.WalletId, name);

        await walletRepository.AddAsync(wallet, cancellationToken);
    }
}