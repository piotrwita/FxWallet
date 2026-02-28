using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.ExchangeRates.Exceptions;

internal sealed class InvalidExchangeRateValueException(decimal rate)
    : CustomException($"Exchange rate must be between {ExchangeRate.MinRate} and {ExchangeRate.MaxRate}. Provided value: {rate}");