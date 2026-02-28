namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class CurrencyMismatchException(string expectedCurrency, string actualCurrency)
    : CustomException($"Cannot perform operation with different currencies. Expected {expectedCurrency}, got {actualCurrency}.");
