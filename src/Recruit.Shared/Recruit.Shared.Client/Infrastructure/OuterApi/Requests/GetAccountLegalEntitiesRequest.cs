using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetAccountLegalEntitiesRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"employeraccounts/{accountId}/legalentities";
}