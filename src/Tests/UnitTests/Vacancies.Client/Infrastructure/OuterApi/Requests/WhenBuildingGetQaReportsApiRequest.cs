using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingGetQaReportsApiRequest
{
    [Test]
    public void Then_The_Correct_Url_Is_Generated()
    {
        var actual = new GetQaReportsApiRequest();

        actual.GetUrl.Should().Be("reports/");
    }
}
