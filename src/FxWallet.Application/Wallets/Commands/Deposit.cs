using FxWallet.Application.Abstractions;

namespace FxWallet.Application.Wallets.Commands;

public sealed record Deposit(Guid WalletId, string CurrencyCode, decimal Amount) : ICommand;