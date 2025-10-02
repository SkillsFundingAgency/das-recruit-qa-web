using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewSummaryRequest : IGetApiRequest
{
    public string GetUrl => "VacancyReviews/summary";
}