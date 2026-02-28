namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class InvalidCurrencyCodeLengthException(string code)
    : CustomException($"Currency code must be exactly 3 characters. Provided code: {code}");
