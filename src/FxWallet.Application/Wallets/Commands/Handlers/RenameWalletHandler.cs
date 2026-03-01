using FxWallet.Application.Abstractions;
using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Application.Wallets.Commands.Handlers;

internal sealed class RenameWalletHandler(IWalletRepository walletRepository) : ICommandHandler<RenameWallet>
{
    public async Task HandleAsync(RenameWallet command, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetByIdAsync(command.WalletId, cancellationToken);
        if (wallet is null)
        {
            throw new WalletNotFoundException(command.WalletId);
        }

        var newName = new WalletName(command.Name);
        wallet.Rename(newName);

        await walletRepository.UpdateAsync(wallet, cancellationToken);
    }
}