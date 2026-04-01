using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingGetGenerateQaReportApiRequest
{
    [Test]
    public void Then_The_Correct_Url_Is_Generated()
    {
        var reportId = Guid.NewGuid();

        var actual = new GetGenerateQaReportApiRequest(reportId);

        actual.GetUrl.Should().Be($"reports/generate/{reportId}");
    }
}
