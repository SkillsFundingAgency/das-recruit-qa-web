using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class WhenBuildingGetBlockedProvidersRequest
{
    [Test]
    public void Then_The_GetBlockedProvidersRequest_Is_Correctly_Built()
    {
        var actual = new GetBlockedProvidersRequest();

        actual.GetUrl.Should().Be("blockedorganisations/providers");
    }
}