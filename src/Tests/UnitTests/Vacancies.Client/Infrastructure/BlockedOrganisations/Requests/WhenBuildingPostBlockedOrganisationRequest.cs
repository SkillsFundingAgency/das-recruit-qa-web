using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class WhenBuildingPostBlockedOrganisationRequest
{
    
    [Test, AutoData]
    public void Then_The_PostBlockedOrganisationRequest_Is_Correctly_Built(BlockedOrganisation organisation)
    {
        var dto = (BlockedOrganisationDto)organisation;
        var actual = new PostBlockedOrganisationRequest(dto);

        actual.PostUrl.Should().Be("blockedorganisations");
        actual.Data.Should().BeEquivalentTo(dto);
    }
}