using AutoFixture.NUnit4;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenBuildingPostUpdateVacancyRequest
{
    [Test, AutoData]
    public void Then_PostUpdateVacancyRequest_Has_Correct_Url_And_Data(VacancyQaFieldUpdate data)
    {
        var actual = new PostUpdateVacancyRequest(data);

        actual.PostUrl.Should().Be($"vacancies/update-from-qa/{data.Id}");
        actual.Data.Should().Be(data);
    }
}