using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetProviderApplicationReviewsCountApiRequest(long Ukprn, List<long> VacancyReferences) : IPostApiRequest
{
    public string PostUrl
    {
        get
        {
            return $"providers/{Ukprn}/count";
        }
    }

    public object Data { get; set; } = VacancyReferences;
}