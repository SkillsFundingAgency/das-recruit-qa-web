using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetProviderDashboardCountApiRequest(long Ukprn) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            return $"providers/{Ukprn}/dashboard";
        }
    }
}