using FxWallet.Application.Abstractions;

namespace FxWallet.Application.Wallets.Commands;

public sealed record RenameWallet(Guid WalletId, string Name) : ICommand;