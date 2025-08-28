using System;
using System.Web;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewsAssignedToUserRequest(string userId, DateTime assignationExpiry) : IGetApiRequest
{
    public string GetUrl => $"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews?assignationExpiry={assignationExpiry}";
}