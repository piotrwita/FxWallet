using FxWallet.Application.Abstractions;

namespace FxWallet.Application.Wallets.Commands;

public sealed record CreateWallet(Guid WalletId, string Name) : ICommand;