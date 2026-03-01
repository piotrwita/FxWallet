using FxWallet.Application.Abstractions;
using FxWallet.Domain.DomainServices.Conversion;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Application.Wallets.Commands.Handlers;

internal sealed class ExchangeHandler(
    IWalletRepository walletRepository,
    ICurrencyConverter currencyConverter) : ICommandHandler<Exchange>
{
    public async Task HandleAsync(Exchange command, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetByIdAsync(command.WalletId, cancellationToken);
        if (wallet is null)
        {
            throw new WalletNotFoundException(command.WalletId);
        }

        var fromCurrency = Currency.FromCode(command.FromCurrencyCode);
        var toCurrency = Currency.FromCode(command.ToCurrencyCode);
        var fromAmount = Money.Create(command.FromAmount, fromCurrency);

        await wallet.ExchangeAsync(fromAmount, toCurrency, currencyConverter, cancellationToken);

        await walletRepository.UpdateAsync(wallet, cancellationToken);
    }
}