using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenConstructingGetProvidersUrl
{
    [Test]
    public void Then_It_Is_Correctly_Constructed()
    {
        //Arrange
        var actual = new GetProvidersRequest();
            
        //Assert
        actual.GetUrl.Should().Be("providers");
    }
}