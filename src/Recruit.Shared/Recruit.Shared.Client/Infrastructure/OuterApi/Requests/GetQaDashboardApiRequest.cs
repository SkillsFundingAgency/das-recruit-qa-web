using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetQaDashboardApiRequest : IGetApiRequest
{
    public string GetUrl => "dashboard";
}
