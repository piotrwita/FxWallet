using System.Xml.Serialization;
using FxWallet.Infrastructure.ExchangeRates.Dtos;
using Microsoft.Extensions.Logging;

namespace FxWallet.Infrastructure.ExchangeRates.Serialization;

internal sealed class NbpXmlDeserializer(ILogger<NbpXmlDeserializer> logger) : IXmlDeserializer
{
    public NbpExchangeRatesResponseDto? Deserialize(string xmlContent)
    {
        try
        {
            XmlSerializer serializer = new(typeof(NbpExchangeRatesResponseDto));
            using StringReader reader = new(xmlContent);
            return (NbpExchangeRatesResponseDto?)serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to deserialize XML content from NBP API");
            return null;
        }
    }
}