using System.Web;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingGetVacancyReviewsAssignedToUserRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_Correctly(string userId, DateTime assignationExpiry, string status)
    {
        var actual = new GetVacancyReviewsAssignedToUserRequest(userId + "@%$£" + userId, assignationExpiry, status);

        actual.GetUrl.Should().Be($"users/{HttpUtility.UrlEncode(userId + "@%$£" + userId)}/VacancyReviews?assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}&status={status}");
    }
}