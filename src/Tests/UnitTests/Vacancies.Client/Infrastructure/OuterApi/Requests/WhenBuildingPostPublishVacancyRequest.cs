using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingPostPublishVacancyRequest
{
    [Test]
    public void Then_PostPublishVacancyRequest_Has_Correct_Url()
    {
        var id = Guid.NewGuid();

        var actual = new PostPublishVacancyRequest(id);

        actual.PostUrl.Should().Be($"vacancies/publish/{id}");
    }
}