using FxWallet.Domain.Shared;
using FxWallet.Domain.Wallets;
using FxWallet.Infrastructure.Data.Serialization;
using Shouldly;

namespace FxWallet.Tests.Unit.Infrastructure.Data.Serialization;

public sealed class WalletSerializerTests
{
    [Fact]
    public void Given_Empty_Wallet_When_Serializing_And_Deserializing_Then_Should_Preserve_All_Properties()
    {
        var originalWallet = Wallet.Create(
            new WalletId(Guid.NewGuid()),
            new WalletName("My Wallet"));

        var json = WalletSerializer.Serialize(originalWallet);
        var deserializedWallet = WalletSerializer.Deserialize(json);

        deserializedWallet.ShouldNotBeNull();
        deserializedWallet.Id.ShouldBe(originalWallet.Id);
        deserializedWallet.Name.ShouldBe(originalWallet.Name);
        deserializedWallet.Balances.Count.ShouldBe(0);
    }

    [Fact]
    public void Given_Wallet_With_Balance_When_Serializing_And_Deserializing_Then_Should_Preserve_Balance()
    {
        var originalWallet = Wallet.Create(
            new WalletId(Guid.NewGuid()),
            new WalletName("My Wallet"));
        originalWallet.Deposit(Money.Create(100m, Currency.FromCode("USD")));
        originalWallet.Deposit(Money.Create(200m, Currency.FromCode("EUR")));

        var json = WalletSerializer.Serialize(originalWallet);
        var deserializedWallet = WalletSerializer.Deserialize(json);

        deserializedWallet.Balances.Count.ShouldBe(2);
        deserializedWallet.Balances.ShouldContain(b => b.Balance.Currency.Code == "USD" && b.Balance.Amount == 100m);
        deserializedWallet.Balances.ShouldContain(b => b.Balance.Currency.Code == "EUR" && b.Balance.Amount == 200m);
    }
}
