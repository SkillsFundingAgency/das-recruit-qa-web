using Esfa.Recruit.Vacancies.Client.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Providers;

public class NationalMinimumWageProviderTests
{
    private readonly NationalMinimumWageProvider _provider;
    private readonly Mock<ILogger<NationalMinimumWageProvider>> _mockLogger;
        
    public NationalMinimumWageProviderTests()
    {
        _mockLogger = new Mock<ILogger<NationalMinimumWageProvider>>();
        _provider = new NationalMinimumWageProvider(_mockLogger.Object);
    }

    [Fact]
    public void ShouldPickCorrectWageRangeBasedOnDate()
    {
        var testDate = new DateTime(2017, 5, 22);
        var wagePeriod = _provider.GetWagePeriod(testDate);

        wagePeriod.ApprenticeshipMinimumWage.Should().Be(3.50m);
        wagePeriod.NationalMinimumWageLowerBound.Should().Be(4.05m);
        wagePeriod.NationalMinimumWageUpperBound.Should().Be(7.50m);
    }
        
    [Fact]
    public void StartDateShouldBeInclusive()
    {
        var testDate = new DateTime(2018, 4, 1);
        var wagePeriod = _provider.GetWagePeriod(testDate);

        wagePeriod.ApprenticeshipMinimumWage.Should().Be(3.70m);
        wagePeriod.NationalMinimumWageLowerBound.Should().Be(4.20m);
        wagePeriod.NationalMinimumWageUpperBound.Should().Be(7.83m);
    }
        
    [Fact]
    public void EndDateShouldBeInclusive()
    {
        var testDate = new DateTime(2018, 3, 31);
        var wagePeriod = _provider.GetWagePeriod(testDate);

        wagePeriod.ApprenticeshipMinimumWage.Should().Be(3.50m);
        wagePeriod.NationalMinimumWageLowerBound.Should().Be(4.05m);
        wagePeriod.NationalMinimumWageUpperBound.Should().Be(7.50m);
    }
        
    [Fact]
    public void ShouldIgnoreTimePartOfDateTime()
    {
        var testDate = new DateTime(2018, 3, 31, 13, 22, 11);
        var wagePeriod = _provider.GetWagePeriod(testDate);

        wagePeriod.ApprenticeshipMinimumWage.Should().Be(3.5m);
        wagePeriod.NationalMinimumWageLowerBound.Should().Be(4.05m);
        wagePeriod.NationalMinimumWageUpperBound.Should().Be(7.5m);
    }
}