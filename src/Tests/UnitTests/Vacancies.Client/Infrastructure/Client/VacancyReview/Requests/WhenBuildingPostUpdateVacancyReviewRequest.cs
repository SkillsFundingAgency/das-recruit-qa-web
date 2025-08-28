using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;
using SFA.DAS.Encoding;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingPostUpdateVacancyReviewRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built_And_Data_Sent(
        [Frozen]Mock<IEncodingService> encodingService)
    {
        encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(123456);
        var fixture = new Fixture();
        var vReview = fixture
            .Build<Recruit.Vacancies.Client.Domain.Entities.VacancyReview>()
            .With(c=>c.AutomatedQaOutcome, new RuleSetOutcome())
            .Create();
        
        var actual = new PostUpdateVacancyReviewRequest(VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object));

        actual.PostUrl.Should().Be($"VacancyReviews/{vReview.Id}/update");
        ((VacancyReviewDto)actual.Data).Should().BeEquivalentTo(VacancyReviewDto.MapVacancyReviewDto(vReview, encodingService.Object));
    }
}