using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewSummaryRequest : IGetApiRequest
{
    public string GetUrl => "VacancyReviews/summary";
}