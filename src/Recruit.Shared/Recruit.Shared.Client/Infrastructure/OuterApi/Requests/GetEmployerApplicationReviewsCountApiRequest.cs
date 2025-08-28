using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record GetEmployerApplicationReviewsCountApiRequest(long AccountId, List<long> VacancyReferences, string ApplicationSharedFilteringStatus) : IPostApiRequest
{
    public string PostUrl => $"employerAccounts/{AccountId}/count?applicationSharedFilteringStatus={ApplicationSharedFilteringStatus}";

    public object Data { get; set; } = VacancyReferences;
}