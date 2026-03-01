using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Wallets.Exceptions;

public sealed class WalletNotFoundException(Guid walletId)
    : CustomException($"Wallet with ID '{walletId}' not found.");