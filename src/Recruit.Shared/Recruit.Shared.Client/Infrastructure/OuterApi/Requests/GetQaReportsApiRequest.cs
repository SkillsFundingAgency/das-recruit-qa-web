using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetQaReportsApiRequest : IGetApiRequest
{
    public string GetUrl => "reports/";
}
