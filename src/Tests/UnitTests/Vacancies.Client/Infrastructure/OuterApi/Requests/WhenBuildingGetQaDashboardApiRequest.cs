using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

[TestFixture]
internal class WhenBuildingGetQaDashboardApiRequest
{
    [Test]
    public void Then_The_Correct_Url_Is_Generated()
    {
        var actual = new GetQaDashboardApiRequest();
        actual.GetUrl.Should().Be("dashboard");
    }
}