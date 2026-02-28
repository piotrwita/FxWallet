using System.Xml.Serialization;

namespace FxWallet.Infrastructure.ExchangeRates.Dtos;

[XmlRoot("ArrayOfExchangeRatesTable")]
internal sealed class NbpExchangeRatesResponseDto
{
    [XmlElement("ExchangeRatesTable")]
    public NbpExchangeRatesTableDto ExchangeRatesTable { get; init; } = null!;
}

[XmlType("ExchangeRatesTable")]
internal sealed record NbpExchangeRatesTableDto
{
    [XmlElement("EffectiveDate")]
    public DateTime EffectiveDate { get; init; }

    [XmlArray("Rates")]
    [XmlArrayItem("Rate")]
    public List<NbpRateDto> Rates { get; init; } = [];
}

[XmlType("Rate")]
internal sealed record NbpRateDto
{
    [XmlElement("Code")]
    public string Code { get; init; } = string.Empty;

    [XmlElement("Mid")]
    public decimal Mid { get; init; }
}