using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class GetBlockedEmployersRequest : IGetApiRequest
{
    public string GetUrl => "blockedorganisations/employers";
}