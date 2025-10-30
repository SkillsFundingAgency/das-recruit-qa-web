using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IBlockedOrganisationQuery
{
    Task<BlockedOrganisation> GetByOrganisationIdAsync(string organisationId);
    Task<List<BlockedOrganisationSummary>> GetAllBlockedProvidersAsync();
    Task<List<BlockedOrganisationSummary>> GetAllBlockedEmployersAsync();
}