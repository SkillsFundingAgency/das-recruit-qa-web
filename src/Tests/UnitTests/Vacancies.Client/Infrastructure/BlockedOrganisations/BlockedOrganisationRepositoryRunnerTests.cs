using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations;

public class BlockedOrganisationRepositoryRunnerTests
{
    [Test, MoqAutoData]
    public async Task When_Calling_CreateAsync_Then_All_Repositories_Are_Called(
        BlockedOrganisation organisation,
        Mock<IBlockedOrganisationRepository> repository1,
        Mock<IBlockedOrganisationRepository> repository2)
    {
        var repositories = new List<IBlockedOrganisationRepository> { repository1.Object, repository2.Object };
        var runner = new BlockedOrganisationRepositoryRunner(repositories);

        await runner.CreateAsync(organisation);

        repository1.Verify(x => x.CreateAsync(organisation), Times.Once);
        repository2.Verify(x => x.CreateAsync(organisation), Times.Once);
    }
}
