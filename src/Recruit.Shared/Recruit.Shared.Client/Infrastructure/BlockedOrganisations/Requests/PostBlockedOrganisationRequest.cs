using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;

public class PostBlockedOrganisationRequest(BlockedOrganisationDto organisation) : IPostApiRequest
{
    public string PostUrl => "blockedorganisations";
    public object Data { get; set; } = organisation;
}