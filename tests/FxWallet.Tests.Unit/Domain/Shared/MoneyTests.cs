using FxWallet.Domain.Shared;
using FxWallet.Domain.Shared.Exceptions;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.Shared;

public sealed class MoneyTests
{
    [Fact]
    public void Given_Valid_Amount_And_Currency_When_Creating_Money_Then_Should_Create_Money()
    {
        var amount = 100m;
        var currency = Currency.FromCode("USD");

        var money = Money.Create(amount, currency);

        money.ShouldNotBeNull();
        money.Amount.ShouldBe(amount);
        money.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public void Given_Zero_Amount_When_Creating_Money_Then_Should_Create_Money()
    {
        var amount = 0m;
        var currency = Currency.FromCode("EUR");

        var money = Money.Create(amount, currency);

        money.ShouldNotBeNull();
        money.Amount.ShouldBe(0m);
    }

    [Fact]
    public void Given_Negative_Amount_When_Creating_Money_Then_Should_Throw_NegativeAmountException()
    {
        var amount = -10m;
        var currency = Currency.FromCode("USD");

        Should.Throw<NegativeAmountException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Given_Valid_Amounts_When_Adding_Money_Then_Should_Add_Amounts()
    {
        var money1 = Money.Create(100m, Currency.FromCode("USD"));
        var money2 = Money.Create(50m, Currency.FromCode("USD"));

        var result = money1.Add(money2);

        result.ShouldNotBeNull();
        result.Amount.ShouldBe(150m);
        result.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public void Given_Different_Currencies_When_Adding_Money_Then_Should_Throw_CurrencyMismatchException()
    {
        var usdMoney = Money.Create(100m, Currency.FromCode("USD"));
        var eurMoney = Money.Create(50m, Currency.FromCode("EUR"));

        Should.Throw<CurrencyMismatchException>(() => usdMoney.Add(eurMoney));
    }

    [Fact]
    public void Given_Null_Money_When_Adding_Then_Should_Throw_ArgumentNullException()
    {
        var money = Money.Create(100m, Currency.FromCode("USD"));

        Should.Throw<ArgumentNullException>(() => money.Add(null!));
    }

    [Fact]
    public void Given_Valid_Amounts_When_Subtracting_Money_Then_Should_Subtract_Amounts()
    {
        var money1 = Money.Create(100m, Currency.FromCode("USD"));
        var money2 = Money.Create(30m, Currency.FromCode("USD"));

        var result = money1.Subtract(money2);

        result.ShouldNotBeNull();
        result.Amount.ShouldBe(70m);
        result.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public void Given_Equal_Amounts_When_Subtracting_Money_Then_Should_Return_Zero()
    {
        var money1 = Money.Create(100m, Currency.FromCode("USD"));
        var money2 = money1;

        var result = money1.Subtract(money2);

        result.Amount.ShouldBe(0m);
    }

    [Fact]
    public void Given_Different_Currencies_When_Subtracting_Money_Then_Should_Throw_CurrencyMismatchException()
    {
        var usdMoney = Money.Create(100m, Currency.FromCode("USD"));
        var eurMoney = Money.Create(50m, Currency.FromCode("EUR"));

        Should.Throw<CurrencyMismatchException>(() => usdMoney.Subtract(eurMoney));
    }

    [Fact]
    public void Given_Larger_Amount_When_Subtracting_Money_Then_Should_Throw_NegativeResultException()
    {
        var money1 = Money.Create(50m, Currency.FromCode("USD"));
        var money2 = Money.Create(100m, Currency.FromCode("USD"));

        Should.Throw<NegativeResultException>(() => money1.Subtract(money2));
    }

    [Fact]
    public void Given_Null_Money_When_Subtracting_Then_Should_Throw_ArgumentNullException()
    {
        var money = Money.Create(100m, Currency.FromCode("USD"));

        Should.Throw<ArgumentNullException>(() => money.Subtract(null!));
    }
}