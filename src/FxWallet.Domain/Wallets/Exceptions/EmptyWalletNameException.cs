using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Wallets.Exceptions;

internal sealed class EmptyWalletNameException()
    : CustomException("Wallet name cannot be empty.");
