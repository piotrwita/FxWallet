using FxWallet.Domain.DomainServices.Conversion;
using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;
using Moq;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.Wallets;

public sealed class WalletTests
{
    [Fact]
    public void Given_Valid_Id_And_Name_When_Creating_Wallet_Then_Should_Create_Wallet()
    {
        var id = new WalletId(Guid.NewGuid());
        var name = new WalletName("My Wallet");

        var wallet = Wallet.Create(id, name);

        wallet.ShouldNotBeNull();
        wallet.Id.ShouldBe(id);
        wallet.Name.ShouldBe(name);
        wallet.Balances.Count.ShouldBe(0);
    }

    [Fact]
    public void Given_Valid_Name_When_Renaming_Wallet_Then_Should_Change_Name()
    {
        var wallet = CreateWallet();
        var newName = new WalletName("New Name");

        wallet.Rename(newName);

        wallet.Name.ShouldBe(newName);
    }

    [Fact]
    public void Given_New_Currency_When_Depositing_Then_Should_Create_Balance()
    {
        var wallet = CreateWallet();
        var amount = Money.Create(100m, Currency.FromCode("USD"));

        wallet.Deposit(amount);

        wallet.Balances.Count.ShouldBe(1);
        wallet.Balances[0].Balance.Amount.ShouldBe(100m);
        wallet.Balances[0].Balance.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public void Given_Existing_Balance_When_Withdrawing_Then_Should_Decrease_Balance()
    {
        var wallet = CreateWallet();
        var initialAmount = Money.Create(100m, Currency.FromCode("USD"));
        wallet.Deposit(initialAmount);
        var withdrawAmount = Money.Create(30m, Currency.FromCode("USD"));

        wallet.Withdraw(withdrawAmount);

        wallet.Balances.Count.ShouldBe(1);
        wallet.Balances[0].Balance.Amount.ShouldBe(70m);
    }

    [Fact]
    public void Given_Zero_Balance_After_Withdrawing_Then_Should_Remove_Balance()
    {
        var wallet = CreateWallet();
        var initialAmount = Money.Create(100m, Currency.FromCode("USD"));
        wallet.Deposit(initialAmount);
        var withdrawAmount = Money.Create(100m, Currency.FromCode("USD"));

        wallet.Withdraw(withdrawAmount);

        wallet.Balances.Count.ShouldBe(0);
    }

    [Fact]
    public void Given_Multiple_Currencies_When_Withdrawing_All_From_One_Then_Should_Remove_Only_That_Balance()
    {
        var wallet = CreateWallet();
        wallet.Deposit(Money.Create(100m, Currency.FromCode("USD")));
        wallet.Deposit(Money.Create(200m, Currency.FromCode("EUR")));

        wallet.Withdraw(Money.Create(100m, Currency.FromCode("USD")));

        wallet.Balances.Count.ShouldBe(1);
        wallet.Balances.ShouldNotContain(b => b.Balance.Currency.Code == "USD");
        wallet.Balances.ShouldContain(b => b.Balance.Currency.Code == "EUR" && b.Balance.Amount == 200m);
    }

    [Fact]
    public void Given_Non_Existent_Currency_When_Withdrawing_Then_Should_Throw_BalanceNotFoundException()
    {
        var wallet = CreateWallet();
        var withdrawAmount = Money.Create(100m, Currency.FromCode("USD"));

        Should.Throw<BalanceNotFoundException>(() => wallet.Withdraw(withdrawAmount));
    }

    [Fact]
    public async Task Given_Different_Currencies_When_Exchanging_Then_Should_Convert_And_Update_Balances()
    {
        var wallet = CreateWallet();
        var fromAmount = Money.Create(100m, Currency.FromCode("USD"));
        wallet.Deposit(fromAmount);
        var toCurrency = Currency.FromCode("EUR");
        SetupConverter("USD", "EUR", 200m);

        await wallet.ExchangeAsync(fromAmount, toCurrency, _converterMock.Object);

        wallet.Balances.Count.ShouldBe(1);
        wallet.Balances[0].Balance.Currency.Code.ShouldBe("EUR");
        wallet.Balances[0].Balance.Amount.ShouldBe(200m);
    }

    [Fact]
    public async Task Given_Same_Currency_When_Exchanging_Then_Should_Throw_CannotExchangeSameCurrencyException()
    {
        var wallet = CreateWallet();
        var amount = Money.Create(100m, Currency.FromCode("USD"));
        wallet.Deposit(amount);

        await Should.ThrowAsync<CannotExchangeSameCurrencyException>(
            () => wallet.ExchangeAsync(amount, Currency.FromCode("USD"), _converterMock.Object));
    }

    [Fact]
    public void Given_Multiple_Balances_When_Getting_Balances_Then_Should_Return_All_Balances()
    {
        var wallet = CreateWallet();
        wallet.Deposit(Money.Create(100m, Currency.FromCode("USD")));
        wallet.Deposit(Money.Create(200m, Currency.FromCode("EUR")));

        wallet.Balances.Count.ShouldBe(2);
        wallet.Balances.ShouldContain(b => b.Balance.Currency.Code == "USD" && b.Balance.Amount == 100m);
        wallet.Balances.ShouldContain(b => b.Balance.Currency.Code == "EUR" && b.Balance.Amount == 200m);
    }

    #region Arrange

    private readonly Mock<ICurrencyConverter> _converterMock;

    public WalletTests()
    {
        _converterMock = new Mock<ICurrencyConverter>();
    }

    private Wallet CreateWallet()
    {
        return Wallet.Create(new WalletId(Guid.NewGuid()), new WalletName("Test Wallet"));
    }

    private void SetupConverter(string fromCurrency, string toCurrency, decimal resultAmount)
    {
        var result = Money.Create(resultAmount, Currency.FromCode(toCurrency));
        _converterMock
            .Setup(c => c.ConvertAsync(
                It.Is<Money>(m => m.Currency.Code == fromCurrency),
                It.Is<Currency>(curr => curr.Code == toCurrency),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }

    #endregion
}
