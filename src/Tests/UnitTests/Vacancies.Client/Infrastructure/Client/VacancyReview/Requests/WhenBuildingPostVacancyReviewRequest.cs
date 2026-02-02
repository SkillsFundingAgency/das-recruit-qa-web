using System.Collections.Generic;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;
using SFA.DAS.Encoding;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingPostVacancyReviewRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Is_Correctly_Built_And_Data_Populated(
        [Frozen]Mock<IEncodingService> encodingService)
    {
        encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(123456);
        var fixture = new Fixture();
        var vReview = fixture
            .Build<Recruit.Vacancies.Client.Domain.Entities.VacancyReview>()
            .With(c=>c.AutomatedQaOutcome, new RuleSetOutcome())
            .Create();
        
        var actual = new PostVacancyReviewRequest(vReview.Id, VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object));

        actual.PostUrl.Should().Be($"VacancyReviews/{vReview.Id}");
        ((VacancyReviewDto)actual.Data).Should().BeEquivalentTo(VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object));
    }
    
    [Test, MoqAutoData]
    public void Then_The_Request_Is_Correctly_Built_And_Data_Populated_With_No_Multiple_Location(
        string fieldIdentifier,
        string fieldIdentifier2,
        [Frozen]Mock<IEncodingService> encodingService)
    {
        encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(123456);
        var fixture = new Fixture();
        var vacancySnapshot = fixture.Build<Vacancy>()
            .With(c => c.EmployerLocation, (Address)null)
            .With(c => c.EmployerLocationOption, (AvailableWhere?)null)
            .Create();
        var vReview = fixture
            .Build<Recruit.Vacancies.Client.Domain.Entities.VacancyReview>()
            .With(c=>c.AutomatedQaOutcome, new RuleSetOutcome())
            .With(c=>c.VacancySnapshot, vacancySnapshot)
            .With(c=>c.ManualQaFieldIndicators,
                [
                    new ManualQaFieldIndicator { IsChangeRequested = true, FieldIdentifier = fieldIdentifier },
                    new ManualQaFieldIndicator { IsChangeRequested = false, FieldIdentifier = fieldIdentifier2 }
                ])
            .Create();
        
        var actual = new PostVacancyReviewRequest(vReview.Id, VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object));

        actual.PostUrl.Should().Be($"VacancyReviews/{vReview.Id}");
        ((VacancyReviewDto)actual.Data).Should().BeEquivalentTo(VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object), options=> options
            .Excluding(c=>c.EmployerLocationOption)
            .Excluding(c=>c.EmployerLocations)
        );
        ((VacancyReviewDto)actual.Data).EmployerLocationOption.Should().Be(AvailableWhere.OneLocation);
        ((VacancyReviewDto)actual.Data).EmployerLocations.Should().BeEquivalentTo([vacancySnapshot.EmployerLocation]);
        ((VacancyReviewDto)actual.Data).ManualQaFieldIndicators.Should().BeEquivalentTo([fieldIdentifier]);
        
    }
}