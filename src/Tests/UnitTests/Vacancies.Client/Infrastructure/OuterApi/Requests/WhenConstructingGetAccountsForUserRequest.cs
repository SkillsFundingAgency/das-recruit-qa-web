using System.Web;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenConstructingGetAccountsForUserRequest
{
    [Test, AutoData]
    public void Then_It_Is_Correctly_Constructed(string userId, string email)
    {
        //Arrange
        var actual = new GetUserAccountsRequest(userId, email);
            
        //Assert
        actual.GetUrl.Should().Be($"accountusers/{userId}/accounts?email={HttpUtility.UrlEncode(email)}");
    }
}