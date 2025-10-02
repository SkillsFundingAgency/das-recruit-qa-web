using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetProvidersRequest : IGetApiRequest
{
    public string GetUrl => "providers";
}