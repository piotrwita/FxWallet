using FxWallet.Domain.Shared.Exceptions;

namespace FxWallet.Domain.Shared;

public sealed record Currency
{
    public static Currency PLN { get; } = new("PLN");

    public string Code { get; init; }

    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new EmptyCurrencyCodeException();
        }

        if (code.Length != 3)
        {
            throw new InvalidCurrencyCodeLengthException(code);
        }

        string upperCode = code.ToUpperInvariant();
        if (!upperCode.All(char.IsLetter))
        {
            throw new InvalidCurrencyCodeFormatException(code);
        }

        Code = upperCode;
    }

    public static Currency FromCode(string code) => new(code);

    public override string ToString() => Code;
}