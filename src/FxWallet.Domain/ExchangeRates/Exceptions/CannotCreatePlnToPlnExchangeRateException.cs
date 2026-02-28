using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.ExchangeRates.Exceptions;

internal sealed class CannotCreatePlnToPlnExchangeRateException() 
    : CustomException("Cannot create exchange rate from PLN to PLN. Exchange rates must be between different currencies.");