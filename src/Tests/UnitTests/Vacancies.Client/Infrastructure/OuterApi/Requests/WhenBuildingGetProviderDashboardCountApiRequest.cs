using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

[TestFixture]
public class WhenBuildingGetProviderDashboardCountApiRequest
{
    [Test, AutoData]
    public void Then_It_Is_Correctly_Constructed(
        int ukprn)
    {
        var actual = new GetProviderDashboardCountApiRequest(ukprn);

        actual.GetUrl.Should().Be($"providers/{ukprn}/dashboard");
    }
}