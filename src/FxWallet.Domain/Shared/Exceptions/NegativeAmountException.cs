namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class NegativeAmountException(decimal amount)
    : CustomException($"Amount cannot be negative. Provided value: {amount}");
