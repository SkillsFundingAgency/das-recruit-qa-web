using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewRequest(Guid id) : IGetApiRequest
{
    public string GetUrl => $"VacancyReviews/{id}";
}