using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations;

public class BlockedOrganisationService(IRecruitQaOuterApiClient client)  : IBlockedOrganisationRepository, IBlockedOrganisationQuery
{
    public async Task CreateAsync(BlockedOrganisation organisation)
    {
        var request = new PostBlockedOrganisationRequest((BlockedOrganisationDto)organisation);
        await client.Post(request);
    }

    public async Task<BlockedOrganisation> GetByOrganisationIdAsync(string organisationId)
    {
        var request = new GetBlockedOrganisationRequest(organisationId);
        var response = await client.Get<BlockedOrganisationDto>(request);
        return response == null ? null : (BlockedOrganisation)response;
    }

    public async Task<List<BlockedOrganisationSummary>> GetAllBlockedProvidersAsync()
    {
        var request = new GetBlockedProvidersRequest();
        var response = await client.Get<List<BlockedOrganisationSummaryDto>>(request);
        return response?.Select(s => (BlockedOrganisationSummary)s).ToList() ?? new List<BlockedOrganisationSummary>();
    }

    public async Task<List<BlockedOrganisationSummary>> GetAllBlockedEmployersAsync()
    {
        var request = new GetBlockedEmployersRequest();
        var response = await client.Get<List<BlockedOrganisationSummaryDto>>(request);
        return response?.Select(s => (BlockedOrganisationSummary)s).ToList() ?? new List<BlockedOrganisationSummary>();
    }
}