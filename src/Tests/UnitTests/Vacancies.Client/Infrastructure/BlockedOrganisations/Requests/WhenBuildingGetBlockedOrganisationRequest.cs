using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class WhenBuildingGetBlockedOrganisationRequest
{
    [Test, AutoData]
    public void Then_The_GetBlockedOrganisationRequest_Is_Correctly_Built(string organisationId)
    {
        var actual = new GetBlockedOrganisationRequest(organisationId);

        actual.GetUrl.Should().Be($"blockedorganisations/{organisationId}");
    }
}