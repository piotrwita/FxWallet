namespace FxWallet.Infrastructure.Data.Models;

internal sealed class ExchangeRateDbModel
{
    public Guid Id { get; set; }
    public string FromCurrencyCode { get; set; } = string.Empty;
    public string ToCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateOnly EffectiveDate { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}