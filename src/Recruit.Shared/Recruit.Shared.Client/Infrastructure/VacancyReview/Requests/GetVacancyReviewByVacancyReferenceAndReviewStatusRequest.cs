using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(long vacancyReference, string reviewStatus = null) : IGetApiRequest
{
    public string GetUrl => $"{vacancyReference}/VacancyReviews?status={reviewStatus}";
}