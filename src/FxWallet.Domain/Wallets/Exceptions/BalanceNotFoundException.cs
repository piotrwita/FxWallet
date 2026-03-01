using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Wallets.Exceptions;

internal sealed class BalanceNotFoundException(Guid walletId, string currencyCode)
    : CustomException($"Balance for currency '{currencyCode}' not found in wallet '{walletId}'. Cannot withdraw.");
