using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.ExchangeRates.Exceptions;

internal sealed class CannotCreateInverseExchangeRateException() 
    : CustomException("Cannot create inverse exchange rate. Exchange rate must be to PLN (ToCurrency must be PLN) to create an inverse rate.");