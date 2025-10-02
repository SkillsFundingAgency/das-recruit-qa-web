using System.Text.RegularExpressions;
using Recruit.Shared.Web.Configuration;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Shared.Web.Configuration;

public class EnvironmentNamesTests
{
    [Fact]
    public void GetNonProdEnvironmentNamesShouldNotIncludeProd()
    {
        Regex.IsMatch(EnvironmentNames.GetNonProdEnvironmentNamesCommaDelimited(), @"(?<!PRE)PROD\b").Should().BeFalse();
    }

    [Fact]
    public void GetNonProdEnvironmentNamesShouldIncludeEveryNonProdEnvironment()
    {
        EnvironmentNames.GetNonProdEnvironmentNamesCommaDelimited().Should().Be("DEVELOPMENT,AT,TEST,TEST2,DEMO,PREPROD");
    }
}