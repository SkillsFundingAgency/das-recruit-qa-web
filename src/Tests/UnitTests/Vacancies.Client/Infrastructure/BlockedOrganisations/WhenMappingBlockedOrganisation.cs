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

        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(c=>c.BlockedStatus)
            .Excluding(c=>c.OrganisationType)
            .Excluding(c=>c.UpdatedByUser)
        );
        actual.BlockedStatus.Should().Be(source.BlockedStatus.ToString());
        actual.OrganisationType.Should().Be(source.OrganisationType.ToString());
        actual.UpdatedByUserEmail.Should().Be(source.UpdatedByUser.Email);
        actual.UpdatedByUserId.Should().Be(source.UpdatedByUser.UserId);
    }

    [Test, AutoData]
    public void Then_The_Dto_Is_Mapped_To_Source(BlockedOrganisationDto source)
    {
        source.OrganisationType = nameof(OrganisationType.Employer);
        source.BlockedStatus = nameof(BlockedStatus.Blocked);
        var actual = (BlockedOrganisation)source;

        actual.Should().BeEquivalentTo(source, options => options.ComparingEnumsByName()
            .Excluding(c => c.OrganisationType)
            .Excluding(c => c.BlockedStatus)
            .Excluding(c => c.UpdatedByUserEmail)
            .Excluding(c => c.UpdatedByUserId)
        );
        actual.OrganisationType.Should().Be(OrganisationType.Employer);
        actual.BlockedStatus.Should().Be(BlockedStatus.Blocked);
        actual.UpdatedByUser.Email.Should().Be(source.UpdatedByUserEmail);
        actual.UpdatedByUser.UserId.Should().Be(source.UpdatedByUserId);
    }
}