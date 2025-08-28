using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.ApplicationReview.Requests;

public record GetApplicationReviewsByVacancyReferenceApiRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            return $"applicationReviews/vacancyReference/{VacancyReference}";
        }
    }
}