using AutoFixture.NUnit3;
using Esfa.Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenConstructingGetAccountRequest
{
    [Test, AutoData]
    public void Then_It_Is_Correctly_Constructed(long accountId)
    {
        //Arrange
        var actual = new GetAccountRequest(accountId);
            
        //Assert
        actual.GetUrl.Should().Be($"employeraccounts/{accountId}");
    }
}