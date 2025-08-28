using System.Collections.Generic;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

[TestFixture]
public class WhenBuildingGetProviderApplicationReviewsCountApiRequest
{
    [Test, AutoData]
    public void Then_It_Is_Correctly_Constructed(
        int ukprn,
        List<long> vacancyReferences)
    {
        var actual = new GetProviderApplicationReviewsCountApiRequest(ukprn, vacancyReferences);

        actual.PostUrl.Should().Be($"providers/{ukprn}/count");
        ((List<long>)actual.Data).Should().BeEquivalentTo(vacancyReferences);
    }
}