using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.DomainServices.Conversion.Exceptions;

internal sealed class ConversionPolicyNotFoundException(string sourceCurrencyCode, string targetCurrencyCode)
    : CustomException($"No conversion strategy found for {sourceCurrencyCode} to {targetCurrencyCode}");
