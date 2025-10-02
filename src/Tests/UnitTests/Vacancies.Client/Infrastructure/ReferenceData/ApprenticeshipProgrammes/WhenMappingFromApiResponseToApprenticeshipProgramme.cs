using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;

public class WhenMappingFromApiResponseToApprenticeshipProgramme
{
    [Fact]
    public void Then_The_Fields_Are_Correctly_Mapped()
    {
        var fixture = new Fixture();
        var source = fixture.Create<GetTrainingProgrammesResponseItem>();

        var actual = (ApprenticeshipProgramme) source;
            
        actual.Should().BeEquivalentTo(source);
    }
}