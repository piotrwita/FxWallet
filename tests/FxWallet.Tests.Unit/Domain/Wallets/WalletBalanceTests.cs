using FxWallet.Domain.Shared;
using FxWallet.Domain.Shared.Exceptions;
using FxWallet.Domain.Wallets;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.Wallets;

public sealed class WalletBalanceTests
{
    [Fact]
    public void Given_Valid_Balance_When_Creating_WalletBalance_Then_Should_Create_WalletBalance()
    {
        var balance = Money.Create(100m, Currency.FromCode("USD"));

        var walletBalance = WalletBalance.Create(balance);

        walletBalance.ShouldNotBeNull();
        walletBalance.Balance.Amount.ShouldBe(100m);
        walletBalance.Balance.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public void Given_Null_Balance_When_Creating_WalletBalance_Then_Should_Throw_ArgumentNullException()
    {
        Should.Throw<ArgumentNullException>(() => WalletBalance.Create(null!));
    }

    [Fact]
    public void Given_Valid_Amount_When_Depositing_Then_Should_Increase_Balance()
    {
        var initialBalance = Money.Create(100m, Currency.FromCode("USD"));
        var walletBalance = WalletBalance.Create(initialBalance);
        var depositAmount = Money.Create(50m, Currency.FromCode("USD"));

        walletBalance.Deposit(depositAmount);

        walletBalance.Balance.Amount.ShouldBe(150m);
    }

    [Fact]
    public void Given_Different_Currency_When_Depositing_Then_Should_Throw_CurrencyMismatchException()
    {
        var initialBalance = Money.Create(100m, Currency.FromCode("USD"));
        var walletBalance = WalletBalance.Create(initialBalance);
        var depositAmount = Money.Create(50m, Currency.FromCode("EUR"));

        Should.Throw<CurrencyMismatchException>(() => walletBalance.Deposit(depositAmount));
    }

    [Fact]
    public void Given_Valid_Amount_When_Withdrawing_Then_Should_Decrease_Balance()
    {
        var initialBalance = Money.Create(100m, Currency.FromCode("USD"));
        var walletBalance = WalletBalance.Create(initialBalance);
        var withdrawAmount = Money.Create(30m, Currency.FromCode("USD"));

        walletBalance.Withdraw(withdrawAmount);

        walletBalance.Balance.Amount.ShouldBe(70m);
    }

    [Fact]
    public void Given_Different_Currency_When_Withdrawing_Then_Should_Throw_CurrencyMismatchException()
    {
        var initialBalance = Money.Create(100m, Currency.FromCode("USD"));
        var walletBalance = WalletBalance.Create(initialBalance);
        var withdrawAmount = Money.Create(50m, Currency.FromCode("EUR"));

        Should.Throw<CurrencyMismatchException>(() => walletBalance.Withdraw(withdrawAmount));
    }

    [Fact]
    public void Given_Larger_Amount_When_Withdrawing_Then_Should_Throw_NegativeResultException()
    {
        var initialBalance = Money.Create(50m, Currency.FromCode("USD"));
        var walletBalance = WalletBalance.Create(initialBalance);
        var withdrawAmount = Money.Create(100m, Currency.FromCode("USD"));

        Should.Throw<NegativeResultException>(() => walletBalance.Withdraw(withdrawAmount));
    }
}
