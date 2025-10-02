using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Responses;

public class GetVacancyReviewListResponse
{
    public List<VacancyReviewDto> VacancyReviews { get; set; }
}