using System.Web;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingGetVacancyReviewCountByUserFilterRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_With_All_Parameters(string userEmail)
    {
        var actual = new GetVacancyReviewCountByUserFilterRequest(userEmail + "@%$£" + userEmail, true);
        
        actual.GetUrl.Should().Be($"users/VacancyReviews/count?approvedFirstTime=True&userEmail={HttpUtility.UrlEncode(userEmail + "@%$£" + userEmail)}");
    }
    
    [Test, AutoData]
    public void Then_The_Request_Is_Built_With_Optional_Parameters(string userEmail)
    {
        var actual = new GetVacancyReviewCountByUserFilterRequest(userEmail + "@%$£" + userEmail);
        
        actual.GetUrl.Should().Be($"users/VacancyReviews/count?approvedFirstTime=False&userEmail={HttpUtility.UrlEncode(userEmail + "@%$£" + userEmail)}");
    }
}