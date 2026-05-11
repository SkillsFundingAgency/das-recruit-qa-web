using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

[TestFixture]
internal class WhenBuildingGetVacancyByReferenceRequest
{
    [Test]
    public void Then_The_Correct_Url_Is_Generated()
    {
        var vacancyReference = 1000000001L;

        var actual = new GetVacancyByReferenceRequest(vacancyReference);

        actual.GetUrl.Should().Be($"vacancies/by/ref/{vacancyReference}");
    }
}
