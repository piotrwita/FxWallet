using FxWallet.Application.Abstractions;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Application.Wallets.Commands.Handlers;

internal sealed class DepositHandler(IWalletRepository walletRepository) : ICommandHandler<Deposit>
{
    public async Task HandleAsync(Deposit command, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetByIdAsync(command.WalletId, cancellationToken);
        if (wallet is null)
        {
            throw new WalletNotFoundException(command.WalletId);
        }

        var currency = Currency.FromCode(command.CurrencyCode);
        var amount = Money.Create(command.Amount, currency);
        wallet.Deposit(amount);

        await walletRepository.UpdateAsync(wallet, cancellationToken);
    }
}