namespace FxWallet.Infrastructure.Data.Models;

internal sealed class WalletDbModel
{
    public Guid Id { get; set; }
    public string WalletObject { get; set; } = "{}";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}