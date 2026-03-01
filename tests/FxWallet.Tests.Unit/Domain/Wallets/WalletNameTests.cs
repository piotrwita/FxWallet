using FxWallet.Domain.Wallets;
using FxWallet.Domain.Wallets.Exceptions;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.Wallets;

public sealed class WalletNameTests
{
    [Fact]
    public void Given_Valid_Name_When_Creating_WalletName_Then_Should_Create_WalletName()
    {
        var name = "My Wallet";

        var walletName = new WalletName(name);

        walletName.ShouldNotBeNull();
        walletName.Value.ShouldBe(name);
    }

    [Fact]
    public void Given_MaxLength_Name_When_Creating_WalletName_Then_Should_Create_WalletName()
    {
        var name = new string('A', WalletName.MaxLength);

        var walletName = new WalletName(name);

        walletName.ShouldNotBeNull();
        walletName.Value.ShouldBe(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Given_Empty_Or_Whitespace_Name_When_Creating_WalletName_Then_Should_Throw_EmptyWalletNameException(string? name)
    {
        Should.Throw<EmptyWalletNameException>(() => new WalletName(name!));
    }

    [Fact]
    public void Given_Name_Exceeding_MaxLength_When_Creating_WalletName_Then_Should_Throw_WalletNameExceedsMaxLengthException()
    {
        var name = new string('A', WalletName.MaxLength + 1);

        Should.Throw<WalletNameExceedsMaxLengthException>(() => new WalletName(name));
    }
}