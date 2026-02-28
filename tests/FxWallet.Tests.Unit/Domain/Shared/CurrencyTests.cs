using FxWallet.Domain.Shared;
using FxWallet.Domain.Shared.Exceptions;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.Shared;

public sealed class CurrencyTests
{
    [Fact]
    public void Given_Valid_Code_When_Creating_Currency_Then_Should_Create_Currency()
    {
        var code = "USD";

        var currency = new Currency(code);

        currency.ShouldNotBeNull();
        currency.Code.ShouldBe("USD");
    }

    [Theory]
    [InlineData("usd", "USD")]
    [InlineData("UsD", "USD")]
    [InlineData("EUR", "EUR")]
    public void Given_Code_When_Creating_Currency_Then_Should_Convert_To_Uppercase(string inputCode, string expectedCode)
    {
        var currency = new Currency(inputCode);

        currency.Code.ShouldBe(expectedCode);
    }

    [Fact]
    public void Given_Pln_Static_Property_Then_Should_Return_Pln_Currency()
    {
        var pln = Currency.PLN;

        pln.ShouldNotBeNull();
        pln.Code.ShouldBe("PLN");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Given_Empty_Or_Whitespace_Code_When_Creating_Currency_Then_Should_Throw_EmptyCurrencyCodeException(string? code)
    {
        Should.Throw<EmptyCurrencyCodeException>(() => new Currency(code!));
    }

    [Theory]
    [InlineData("US")]
    [InlineData("U")]
    [InlineData("USDX")]
    [InlineData("USDD")]
    public void Given_Invalid_Length_Code_When_Creating_Currency_Then_Should_Throw_InvalidCurrencyCodeLengthException(string code)
    {
        Should.Throw<InvalidCurrencyCodeLengthException>(() => new Currency(code));
    }

    [Theory]
    [InlineData("US1")]
    [InlineData("12D")]
    [InlineData("U$D")]
    [InlineData("US-")]
    public void Given_Code_With_Non_Letter_Characters_When_Creating_Currency_Then_Should_Throw_InvalidCurrencyCodeFormatException(string code)
    {
        Should.Throw<InvalidCurrencyCodeFormatException>(() => new Currency(code));
    }
}
