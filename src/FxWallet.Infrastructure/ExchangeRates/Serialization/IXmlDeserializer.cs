using FxWallet.Infrastructure.ExchangeRates.Dtos;

namespace FxWallet.Infrastructure.ExchangeRates.Serialization;

internal interface IXmlDeserializer
{
    NbpExchangeRatesResponseDto? Deserialize(string xmlContent);
}