using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetRouteRequest : IGetApiRequest
{
    public string GetUrl => "routes";
}