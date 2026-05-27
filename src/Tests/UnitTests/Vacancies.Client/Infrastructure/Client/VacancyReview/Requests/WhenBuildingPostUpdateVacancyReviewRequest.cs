using AutoFixture.NUnit4;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;
using SFA.DAS.Encoding;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingPostUpdateVacancyReviewRequest
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Request_Is_Correctly_Built_And_Data_Sent(
        Recruit.Vacancies.Client.Domain.Entities.VacancyReview vacancyReview,
        [Frozen]Mock<IEncodingService> encodingService)
    {
        // arrange
        encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(123456);
        
        // act
        var actual = new PostUpdateVacancyReviewRequest(VacancyReviewDto.MapVacancyReviewDto(vacancyReview, encodingService.Object));

        // assert
        actual.PostUrl.Should().Be($"VacancyReviews/{vacancyReview.Id}/update");
        ((VacancyReviewDto)actual.Data).Should().BeEquivalentTo(VacancyReviewDto.MapVacancyReviewDto(vacancyReview, encodingService.Object));
    }
}