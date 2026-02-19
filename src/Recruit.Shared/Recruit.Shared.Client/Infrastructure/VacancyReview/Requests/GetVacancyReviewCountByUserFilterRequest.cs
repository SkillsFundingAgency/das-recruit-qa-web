using System.Web;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;

public class GetVacancyReviewCountByUserFilterRequest(string userEmail, bool approvedFirstTime = false) : IGetApiRequest
{
    public string GetUrl => $"users/VacancyReviews/count?approvedFirstTime={approvedFirstTime}&userEmail={HttpUtility.UrlEncode(userEmail)}";
}