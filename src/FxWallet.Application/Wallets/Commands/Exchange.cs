using FxWallet.Application.Abstractions;

namespace FxWallet.Application.Wallets.Commands;

public sealed record Exchange(
    Guid WalletId,
    string FromCurrencyCode,
    decimal FromAmount,
    string ToCurrencyCode) : ICommand;
