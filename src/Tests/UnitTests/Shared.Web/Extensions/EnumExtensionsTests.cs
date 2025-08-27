using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Shared.Web.Extensions;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Shared.Web.Extensions;

public class EnumExtensionsTests
{
    [Theory]
    [InlineData(FilteringOptions.ClosingSoon, true)]
    [InlineData(FilteringOptions.ClosingSoonWithNoApplications, true)]
    [InlineData(FilteringOptions.Closed, false)]
    [InlineData(FilteringOptions.Approved, false)]
    [InlineData(FilteringOptions.Referred, false)]
    [InlineData(FilteringOptions.Submitted, false)]
    public void Check_IsInLiveVacancyOptions(FilteringOptions filteringOptions, bool expectedOutput)
    {
        filteringOptions.IsInLiveVacancyOptions().Should().Be(expectedOutput);
    }
}