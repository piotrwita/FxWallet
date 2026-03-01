using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Wallets.Exceptions;

internal sealed class WalletNameExceedsMaxLengthException()
    : CustomException($"Wallet name cannot exceed {WalletName.MaxLength} characters.");