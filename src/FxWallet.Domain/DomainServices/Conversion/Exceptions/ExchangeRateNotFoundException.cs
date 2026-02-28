using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.DomainServices.Conversion.Exceptions;

internal sealed class ExchangeRateNotFoundException(string currencyCode)
    : CustomException($"Exchange rate to PLN not found for currency: {currencyCode}");