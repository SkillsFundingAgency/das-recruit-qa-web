using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class GetBlockedProvidersRequest : IGetApiRequest
{
    public string GetUrl => "blockedorganisations/byorganisationtype/providers";
}