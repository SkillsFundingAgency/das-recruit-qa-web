using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations;

public class WhenMappingBlockedOrganisationSummary
{
    [Test, AutoData]
    public void Then_The_Source_Is_Mapped_To_Dto(BlockedOrganisationSummary source)
    {
        var actual = (BlockedOrganisationSummaryDto)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_The_Dto_Is_Mapped_To_Source(BlockedOrganisationSummaryDto source)
    {
        var actual = (BlockedOrganisationSummary)source;

        actual.Should().BeEquivalentTo(source);
    }
}