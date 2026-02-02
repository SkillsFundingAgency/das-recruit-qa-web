using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(long vacancyReference, List<ManualQaOutcome> manualOutcome, bool includeNoStatus, string reviewStatus = null) : IGetApiRequest
{
    public string GetUrl => $"vacancies/{vacancyReference}/VacancyReviews?status={reviewStatus}&manualOutcome={string.Join("&manualOutcome=", manualOutcome)}&includeNoStatus={includeNoStatus}";
}