using FxWallet.Application.Abstractions;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Application.Wallets.Commands.Handlers;

internal sealed class WithdrawHandler(IWalletRepository walletRepository) : ICommandHandler<Withdraw>
{
    public async Task HandleAsync(Withdraw command, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetByIdAsync(command.WalletId, cancellationToken);
        if (wallet is null)
        {
            throw new WalletNotFoundException(command.WalletId);
        }

        Money amount = Money.Create(command.Amount, Currency.FromCode(command.CurrencyCode));
        wallet.Withdraw(amount);

        await walletRepository.UpdateAsync(wallet, cancellationToken);
    }
}
