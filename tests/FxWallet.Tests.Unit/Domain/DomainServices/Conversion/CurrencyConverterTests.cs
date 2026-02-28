using FxWallet.Domain.DomainServices.Conversion;
using FxWallet.Domain.DomainServices.Conversion.Exceptions;
using FxWallet.Domain.DomainServices.Conversion.Policies;
using FxWallet.Domain.ExchangeRates;
using FxWallet.Domain.Shared;
using Moq;
using Shouldly;

namespace FxWallet.Tests.Unit.Domain.DomainServices.Conversion;

public sealed class CurrencyConverterTests
{
    [Fact]
    public async Task Given_Same_Currency_When_Converting_Then_Should_Return_Same_Amount()
    {
        var amount = Money.Create(100m, Currency.FromCode("USD"));
        var targetCurrency = Currency.FromCode("USD");

        var result = await _converter.ConvertAsync(amount, targetCurrency);

        result.ShouldNotBeNull();
        result.Amount.ShouldBe(100m);
        result.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public async Task Given_Currency_To_Pln_When_Converting_Then_Should_Convert_To_Pln()
    {
        var amount = Money.Create(100m, Currency.FromCode("USD"));
        var targetCurrency = Currency.PLN;
        SetupExchangeRate("USD", 4.0m);

        var result = await _converter.ConvertAsync(amount, targetCurrency);

        result.ShouldNotBeNull();
        result.Amount.ShouldBe(400m);
        result.Currency.Code.ShouldBe("PLN");
    }

    [Fact]
    public async Task Given_Pln_To_Currency_When_Converting_Then_Should_Convert_From_Pln()
    {
        var amount = Money.Create(400m, Currency.PLN);
        var targetCurrency = Currency.FromCode("USD");
        SetupExchangeRate("USD", 4.0m);

        var result = await _converter.ConvertAsync(amount, targetCurrency);

        result.ShouldNotBeNull();
        result.Amount.ShouldBe(100m);
        result.Currency.Code.ShouldBe("USD");
    }

    [Fact]
    public async Task Given_Currency_To_Currency_When_Converting_Then_Should_Convert_Through_Pln()
    {
        var amount = Money.Create(100m, Currency.FromCode("USD"));
        var targetCurrency = Currency.FromCode("EUR");
        SetupExchangeRate("USD", 4.0m);
        SetupExchangeRate("EUR", 2.0m);

        var result = await _converter.ConvertAsync(amount, targetCurrency);

        result.ShouldNotBeNull();
        result.Currency.Code.ShouldBe("EUR");
        result.Amount.ShouldBe(200m);
    }

    [Fact]
    public async Task Given_Null_Amount_When_Converting_Then_Should_Throw_ArgumentNullException()
    {
        var targetCurrency = Currency.FromCode("USD");

        await Should.ThrowAsync<ArgumentNullException>(() => _converter.ConvertAsync(null!, targetCurrency));
    }

    [Fact]
    public async Task Given_Null_Target_Currency_When_Converting_Then_Should_Throw_ArgumentNullException()
    {
        var amount = Money.Create(100m, Currency.FromCode("USD"));

        await Should.ThrowAsync<ArgumentNullException>(() => _converter.ConvertAsync(amount, null!));
    }

    [Theory]
    [InlineData("USD", "PLN")]
    [InlineData("PLN", "USD")]
    [InlineData("USD", "EUR")]
    public async Task Given_Exchange_Rate_Not_Found_When_Converting_Then_Should_Throw_ExchangeRateNotFoundException(
        string sourceCurrencyCode, string targetCurrencyCode)
    {
        var sourceCurrency = Currency.FromCode(sourceCurrencyCode);
        var targetCurrency = Currency.FromCode(targetCurrencyCode);
        var amount = Money.Create(100m, sourceCurrency);

        _repositoryMock.Setup(r => r.GetCurrentRateToPlnAsync(sourceCurrency, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRate?)null);

        await Should.ThrowAsync<ExchangeRateNotFoundException>(() => _converter.ConvertAsync(amount, targetCurrency));
    }

    #region Arrange

    private readonly Mock<IExchangeRateRepository> _repositoryMock;
    private readonly CurrencyConverter _converter;

    public CurrencyConverterTests()
    {
        _repositoryMock = new Mock<IExchangeRateRepository>();
        var policies = new List<IConversionPolicy>
        {
            new IdentityConversionPolicy(),
            new ToPlnConversionPolicy(_repositoryMock.Object),
            new FromPlnConversionPolicy(_repositoryMock.Object),
            new CrossRateConversionPolicy(_repositoryMock.Object)
        };
        _converter = new CurrencyConverter(policies);
    }

    #endregion

    #region Helpers

    private void SetupExchangeRate(string currencyCode, decimal rate)
    {
        var currency = Currency.FromCode(currencyCode);
        var exchangeRate = ExchangeRate.CreateToPln(currency, rate);
        _repositoryMock.Setup(r => r.GetCurrentRateToPlnAsync(currency, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRate);
    }

    #endregion
}
