using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingPostCloseVacancyRequest
{
    [Test]
    public void Then_PostCloseVacancyRequest_Has_Correct_Url_And_Data()
    {
        var id = Guid.NewGuid();
        var data = new CloseVacancyRequest { ClosureReason = "OtherReason" };

        var actual = new PostCloseVacancyRequest(id, data);

        actual.PostUrl.Should().Be($"vacancies/close/{id}");
        actual.Data.Should().Be(data);
    }
}