using FxWallet.Application.Abstractions;

namespace FxWallet.Application.Wallets.Commands;

public sealed record Withdraw(Guid WalletId, string CurrencyCode, decimal Amount) : ICommand;
