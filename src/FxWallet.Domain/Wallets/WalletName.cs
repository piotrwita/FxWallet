using FxWallet.Domain.Wallets.Exceptions;

namespace FxWallet.Domain.Wallets;

public sealed record WalletName
{
    public const int MaxLength = 30;

    public string Value { get; private init; }

    public WalletName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyWalletNameException();
        }

        string trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
        {
            throw new WalletNameExceedsMaxLengthException();
        }

        Value = value;
    }

    public override string ToString() => Value;
}
