using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations;

public interface IBlockedOrganisationRepositoryRunner
{
    Task CreateAsync(BlockedOrganisation organisation);
}

public class BlockedOrganisationRepositoryRunner(IEnumerable<IBlockedOrganisationRepository> repositories) : IBlockedOrganisationRepositoryRunner
{
    public async Task CreateAsync(BlockedOrganisation organisation)
    {
        foreach (var repository in repositories)
        {
            await repository.CreateAsync(organisation);
        }
    }
}