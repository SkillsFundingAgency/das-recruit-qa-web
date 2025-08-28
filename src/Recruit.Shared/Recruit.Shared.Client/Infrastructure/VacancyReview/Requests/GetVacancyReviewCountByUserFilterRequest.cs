using System.Web;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewCountByUserFilterRequest(string userId, bool approvedFirstTime = false) : IGetApiRequest
{
    public string GetUrl => $"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews/count?approvedFirstTime={approvedFirstTime}";
}