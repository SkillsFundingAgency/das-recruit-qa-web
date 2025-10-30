using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.BlockedOrganisations;

public class BlockedEmployerOrganisations() : QueryProjectionBase(QueryViewType.BlockedEmployerOrganisations.TypeName)
{
    public IEnumerable<BlockedOrganisationSummary> Data { get; set; }
}