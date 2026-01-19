using System;
using System.Web;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewsAssignedToUserRequest(string userId, DateTime assignationExpiry, string status) : IGetApiRequest
{
    public string GetUrl => $"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews?assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}&status={status}";
}