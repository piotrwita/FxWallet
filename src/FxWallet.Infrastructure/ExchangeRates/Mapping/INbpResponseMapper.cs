using FxWallet.Domain.ExchangeRates;
using FxWallet.Infrastructure.ExchangeRates.Dtos;

namespace FxWallet.Infrastructure.ExchangeRates.Mapping;

internal interface INbpResponseMapper
{
    (IEnumerable<ExchangeRate> rates, DateOnly effectiveDate) Map(NbpExchangeRatesResponseDto responseDto);
}
