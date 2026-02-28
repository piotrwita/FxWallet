using System.Xml.Serialization;

namespace FxWallet.Infrastructure.ExchangeRates.Dtos;

[XmlRoot("ArrayOfExchangeRatesTable")]
public sealed class NbpExchangeRatesResponseDto
{
    [XmlElement("ExchangeRatesTable")]
    public NbpExchangeRatesTableDto ExchangeRatesTable { get; init; } = null!;
}

[XmlType("ExchangeRatesTable")]
public sealed record NbpExchangeRatesTableDto
{
    [XmlElement("EffectiveDate")]
    public DateTime EffectiveDate { get; init; }

    [XmlArray("Rates")]
    [XmlArrayItem("Rate")]
    public List<NbpRateDto> Rates { get; init; } = [];
}

[XmlType("Rate")]
public sealed record NbpRateDto
{
    [XmlElement("Code")]
    public string Code { get; init; } = string.Empty;

    [XmlElement("Mid")]
    public decimal Mid { get; init; }
}