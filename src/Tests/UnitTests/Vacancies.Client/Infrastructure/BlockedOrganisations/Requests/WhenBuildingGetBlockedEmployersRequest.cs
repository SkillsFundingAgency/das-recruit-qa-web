using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class WhenBuildingGetBlockedEmployersRequest
{
    [Test]
    public void Then_The_GetBlockedEmployersRequest_Is_Correctly_Built()
    {
        var actual = new GetBlockedEmployersRequest();

        actual.GetUrl.Should().Be("blockedorganisations/employers");
    }
}