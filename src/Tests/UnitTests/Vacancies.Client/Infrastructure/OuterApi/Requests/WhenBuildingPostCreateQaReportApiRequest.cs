using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingPostCreateQaReportApiRequest
{
    [Test, AutoData]
    public void Then_The_Correct_Url_Is_Generated_And_Data_Updated(PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData payload)
    {
        var actual = new PostCreateQaReportApiRequest(payload);

        actual.PostUrl.Should().Be("reports/create");
        actual.Data.Should().Be(payload);
    }
}
