using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.ExchangeRates.Exceptions;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Shared.Exceptions;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.ExchangeRates;

public sealed class ExchangeRateTests
{
    [Fact]
    public void Given_Valid_Currency_And_Rate_When_Creating_To_Pln_Then_Should_Create_Exchange_Rate()
    {
        var usd = Currency.FromCode("USD");
        var rate = 4.25m;

        var exchangeRate = ExchangeRate.CreateToPln(usd, rate);

        exchangeRate.ShouldNotBeNull();
        exchangeRate.FromCurrency.Code.ShouldBe("USD");
        exchangeRate.ToCurrency.Code.ShouldBe("PLN");
        exchangeRate.Rate.ShouldBe(rate);
    }

    [Fact]
    public void Given_Valid_Currency_And_Rate_When_Creating_From_Pln_Then_Should_Create_Exchange_Rate()
    {
        var eur = Currency.FromCode("EUR");
        var rate = 0.23m;

        var exchangeRate = ExchangeRate.CreateFromPln(eur, rate);

        exchangeRate.ShouldNotBeNull();
        exchangeRate.FromCurrency.Code.ShouldBe("PLN");
        exchangeRate.ToCurrency.Code.ShouldBe("EUR");
        exchangeRate.Rate.ShouldBe(rate);
    }

    [Fact]
    public void Given_Pln_Currency_When_Creating_To_Pln_Then_Should_Throw_CannotCreatePlnToPlnExchangeRateException()
    {
        var pln = Currency.PLN;
        var rate = 1.0m;

        Should.Throw<CannotCreatePlnToPlnExchangeRateException>(() => ExchangeRate.CreateToPln(pln, rate));
    }

    [Fact]
    public void Given_Pln_Currency_When_Creating_From_Pln_Then_Should_Throw_CannotCreatePlnToPlnExchangeRateException()
    {
        var pln = Currency.PLN;
        var rate = 1.0m;

        Should.Throw<CannotCreatePlnToPlnExchangeRateException>(() => ExchangeRate.CreateFromPln(pln, rate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.0)]
    [InlineData(1_000_001)]
    public void Given_Invalid_Rate_When_Creating_To_Pln_Then_Should_Throw_InvalidExchangeRateValueException(decimal rate)
    {
        var usd = Currency.FromCode("USD");

        Should.Throw<InvalidExchangeRateValueException>(() => ExchangeRate.CreateToPln(usd, rate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.0)]
    [InlineData(1_000_001)]
    public void Given_Invalid_Rate_When_Creating_From_Pln_Then_Should_Throw_InvalidExchangeRateValueException(decimal rate)
    {
        var eur = Currency.FromCode("EUR");

        Should.Throw<InvalidExchangeRateValueException>(() => ExchangeRate.CreateFromPln(eur, rate));
    }

    [Fact]
    public void Given_Exchange_Rate_To_Pln_When_Creating_Inverse_Then_Should_Create_Inverse_Exchange_Rate()
    {
        var usd = Currency.FromCode("USD");
        var rate = 4.0m;
        var usdToPln = ExchangeRate.CreateToPln(usd, rate);

        var inverseRate = ExchangeRate.CreateInverse(usdToPln);

        inverseRate.ShouldNotBeNull();
        inverseRate.FromCurrency.Code.ShouldBe("PLN");
        inverseRate.ToCurrency.Code.ShouldBe("USD");
        inverseRate.Rate.ShouldBe(0.25m);
    }

    [Fact]
    public void Given_Exchange_Rate_Not_To_Pln_When_Creating_Inverse_Then_Should_Throw_CannotCreateInverseExchangeRateException()
    {
        var plnToEur = ExchangeRate.CreateFromPln(Currency.FromCode("EUR"), 0.25m);

        Should.Throw<CannotCreateInverseExchangeRateException>(() => ExchangeRate.CreateInverse(plnToEur));
    }

    [Fact]
    public void Given_Null_Exchange_Rate_When_Creating_Inverse_Then_Should_Throw_ArgumentNullException()
    {
        Should.Throw<ArgumentNullException>(() => ExchangeRate.CreateInverse(null!));
    }

    [Fact]
    public void Given_Valid_Amount_When_Converting_Then_Should_Convert_Money()
    {
        var usd = Currency.FromCode("USD");
        var rate = 4.0m;
        var usdToPln = ExchangeRate.CreateToPln(usd, rate);
        var amount = Money.Create(100m, usd);

        var converted = usdToPln.Convert(amount);

        converted.ShouldNotBeNull();
        converted.Amount.ShouldBe(400m);
        converted.Currency.Code.ShouldBe("PLN");
    }

    [Fact]
    public void Given_Currency_Mismatch_When_Converting_Then_Should_Throw_CurrencyMismatchException()
    {
        var usd = Currency.FromCode("USD");
        var eur = Currency.FromCode("EUR");
        var rate = 4.0m;
        var usdToPln = ExchangeRate.CreateToPln(usd, rate);
        var eurAmount = Money.Create(100m, eur);

        Should.Throw<CurrencyMismatchException>(() => usdToPln.Convert(eurAmount));
    }

    [Fact]
    public void Given_Null_Amount_When_Converting_Then_Should_Throw_ArgumentNullException()
    {
        var usd = Currency.FromCode("USD");
        var rate = 4.0m;
        var usdToPln = ExchangeRate.CreateToPln(usd, rate);

        Should.Throw<ArgumentNullException>(() => usdToPln.Convert(null!));
    }
}
