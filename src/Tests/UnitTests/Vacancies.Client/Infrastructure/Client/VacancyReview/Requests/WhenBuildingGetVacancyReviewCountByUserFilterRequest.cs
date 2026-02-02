using System.Web;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingGetVacancyReviewCountByUserFilterRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_With_All_Parameters(string userId)
    {
        var actual = new GetVacancyReviewCountByUserFilterRequest(userId + "@%$£" + userId, true);
        
        actual.GetUrl.Should().Be($"users/VacancyReviews/count?approvedFirstTime=True&userId={HttpUtility.UrlEncode(userId + "@%$£" + userId)}");
    }
    
    [Test, AutoData]
    public void Then_The_Request_Is_Built_With_Optional_Parameters(string userId)
    {
        var actual = new GetVacancyReviewCountByUserFilterRequest(userId + "@%$£" + userId);
        
        actual.GetUrl.Should().Be($"users/VacancyReviews/count?approvedFirstTime=False&userId={HttpUtility.UrlEncode(userId + "@%$£" + userId)}");
    }
}