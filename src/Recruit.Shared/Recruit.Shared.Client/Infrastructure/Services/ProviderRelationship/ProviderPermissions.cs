using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.Services.ProviderRelationship;

internal class ProviderPermissions
{
    public IEnumerable<LegalEntityDto> AccountProviderLegalEntities { get; set;}
}