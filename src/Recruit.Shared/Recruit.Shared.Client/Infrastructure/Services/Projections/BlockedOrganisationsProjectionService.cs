using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.BlockedOrganisations;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Projections;

public class BlockedOrganisationsProjectionService(
    ILogger<BlockedOrganisationsProjectionService> logger,
    IBlockedOrganisationQuery repo,
    IQueryStoreWriter queryStoreWriter)
    : IBlockedOrganisationsProjectionService
{
    public async Task RebuildBlockedProviderOrganisations()
    {
        var blockedProviders = await repo.GetAllBlockedProvidersAsync();

        logger.LogInformation($"Found {blockedProviders.Count} currently blocked providers to populate queryStore {nameof(BlockedProviderOrganisations)} document.");
        await queryStoreWriter.UpdateBlockedProviders(blockedProviders);
    }

    public async Task RebuildBlockedEmployerOrganisations()
    {
        var blockedEmployers = await repo.GetAllBlockedEmployersAsync();

        logger.LogInformation($"Found {blockedEmployers.Count} currently blocked employers to populate queryStore {nameof(BlockedEmployerOrganisations)} document.");

        await queryStoreWriter.UpdateBlockedEmployers(blockedEmployers);
    }

    public Task RebuildAllBlockedOrganisationsAsync()
    {
        return Task.WhenAll(RebuildBlockedProviderOrganisations(),RebuildBlockedEmployerOrganisations());
    }
}