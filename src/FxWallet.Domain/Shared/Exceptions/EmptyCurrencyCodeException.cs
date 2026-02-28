namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class EmptyCurrencyCodeException()
    : CustomException("Currency code cannot be null or empty.");
