using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Wallets.Exceptions;

internal sealed class CannotExchangeSameCurrencyException(string currencyCode)
    : CustomException($"Cannot exchange to the same currency. Source and target currencies are both: {currencyCode}");
