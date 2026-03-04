using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class GetBlockedOrganisationRequest(string organisationId) : IGetApiRequest
{
    public string GetUrl => $"blockedorganisations/{organisationId}";
}