using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.ExchangeRates.Exceptions;

internal sealed class SameCurrenciesException(string currencyCode)
    : CustomException($"Cannot create exchange rate. Source and target currencies must be different. Both currencies are: {currencyCode}");