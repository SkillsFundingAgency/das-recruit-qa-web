using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetAccountRequest : IGetApiRequest
{
    private readonly long _accountId;

    public GetAccountRequest(long accountId)
    {
        _accountId = accountId;
    }

    public string GetUrl => $"employeraccounts/{_accountId}";
}