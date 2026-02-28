namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class InvalidCurrencyCodeFormatException(string code)
    : CustomException($"Currency code must contain only letters. Provided code: {code}");
