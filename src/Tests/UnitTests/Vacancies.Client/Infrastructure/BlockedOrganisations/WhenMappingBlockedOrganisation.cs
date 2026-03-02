using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations;

public class WhenMappingBlockedOrganisation
{
    [Test, AutoData]
    public void Then_The_Source_Is_Mapped_To_Dto(BlockedOrganisation source)
    {
        var actual = (BlockedOrganisationDto)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_The_Dto_Is_Mapped_To_Source(BlockedOrganisationDto source)
    {
        var actual = (BlockedOrganisation)source;

        actual.Should().BeEquivalentTo(source);
    }
}