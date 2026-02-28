using FxWallet.Infrastructure.ExchangeRates.Dtos;
using FxWallet.Infrastructure.ExchangeRates.Mapping;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace FxWallet.Tests.Unit.Infrastructure.ExchangeRates.Mapping;

public sealed class NbpResponseMapperTests
{
    [Fact]
    public void Given_Valid_Response_With_Multiple_Rates_When_Mapping_Then_Should_Map_All_Rates_And_EffectiveDate()
    {
        var responseDto = new NbpExchangeRatesResponseDto
        {
            ExchangeRatesTable = new NbpExchangeRatesTableDto
            {
                EffectiveDate = new DateTime(2026, 2, 25),
                Rates =
                [
                    new NbpRateDto { Code = "USD", Mid = 4.0m },
                    new NbpRateDto { Code = "EUR", Mid = 4.5m },
                    new NbpRateDto { Code = "GBP", Mid = 5.0m }
                ]
            }
        };

        var (rates, effectiveDate) = _mapper.Map(responseDto);

        rates.ShouldNotBeNull();
        rates.Count().ShouldBe(3);
        effectiveDate.ShouldBe(new DateOnly(2026, 2, 25));
        rates.ShouldContain(r => r.FromCurrency.Code == "USD" && r.Rate == 4.0m);
        rates.ShouldContain(r => r.FromCurrency.Code == "EUR" && r.Rate == 4.5m);
        rates.ShouldContain(r => r.FromCurrency.Code == "GBP" && r.Rate == 5.0m);
    }

    [Fact]
    public void Given_Null_ExchangeRatesTable_When_Mapping_Then_Should_Return_Empty_Rates()
    {
        var responseDto = new NbpExchangeRatesResponseDto
        {
            ExchangeRatesTable = null!
        };

        var (rates, effectiveDate) = _mapper.Map(responseDto);

        rates.ShouldNotBeNull();
        rates.Count().ShouldBe(0);
        effectiveDate.ShouldBe(DateOnly.FromDateTime(default));
    }

    [Fact]
    public void Given_Invalid_Currency_Code_When_Mapping_Then_Should_Skip_Invalid_Rate()
    {
        var responseDto = new NbpExchangeRatesResponseDto
        {
            ExchangeRatesTable = new NbpExchangeRatesTableDto
            {
                EffectiveDate = new DateTime(2026, 2, 25),
                Rates =
                [
                    new NbpRateDto { Code = "USD", Mid = 4.0m },
                    new NbpRateDto { Code = "INVALID", Mid = 4.5m },
                    new NbpRateDto { Code = "EUR", Mid = 4.5m }
                ]
            }
        };

        var (rates, effectiveDate) = _mapper.Map(responseDto);

        rates.ShouldNotBeNull();
        rates.Count().ShouldBe(2);
        rates.ShouldContain(r => r.FromCurrency.Code == "USD");
        rates.ShouldContain(r => r.FromCurrency.Code == "EUR");
        rates.ShouldNotContain(r => r.FromCurrency.Code == "INVALID");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.0)]
    [InlineData(1_000_001)]
    public void Given_Invalid_Rate_Value_When_Mapping_Then_Should_Skip_Invalid_Rate(decimal invalidRate)
    {
        var responseDto = new NbpExchangeRatesResponseDto
        {
            ExchangeRatesTable = new NbpExchangeRatesTableDto
            {
                EffectiveDate = new DateTime(2026, 2, 25),
                Rates =
                [
                    new NbpRateDto { Code = "USD", Mid = 4.0m },
                    new NbpRateDto { Code = "EUR", Mid = invalidRate },
                    new NbpRateDto { Code = "GBP", Mid = 5.0m }
                ]
            }
        };

        var (rates, effectiveDate) = _mapper.Map(responseDto);

        rates.ShouldNotBeNull();
        rates.Count().ShouldBe(2);
        rates.ShouldContain(r => r.FromCurrency.Code == "USD");
        rates.ShouldContain(r => r.FromCurrency.Code == "GBP");
        rates.ShouldNotContain(r => r.FromCurrency.Code == "EUR");
    }

    #region Arrange

    private readonly NbpResponseMapper _mapper;

    public NbpResponseMapperTests()
    {
        _mapper = new NbpResponseMapper(NullLogger<NbpResponseMapper>.Instance);
    }

    #endregion
}
