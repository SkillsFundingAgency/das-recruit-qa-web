using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.FeatureToggle;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using System.Linq;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;

public class WhenGettingAllApprenticeshipProgrammes
{
    [Test, MoqAutoData]
    public async Task Then_The_Courses_Are_Retrieved_From_The_Api_When_Not_Cached(
        GetTrainingProgrammesResponse apiResponse,
        [Frozen] Mock<ITimeProvider> mockTimeProvider,
        [Frozen] Mock<IRecruitOuterApiClient> outerApiClient)
    {
        outerApiClient
            .Setup(x => x.Get<GetTrainingProgrammesResponse>(It.IsAny<GetTrainingProgrammesRequest>()))
            .ReturnsAsync(apiResponse);
        var cache = new TestHelpers.TestCache();
        var provider = new ApprenticeshipProgrammeProvider(cache, mockTimeProvider.Object, outerApiClient.Object, Mock.Of<IFeature>());
        
        var actual = await provider.GetApprenticeshipProgrammesAsync(true);

        actual.Should().BeEquivalentTo(apiResponse.TrainingProgrammes.Select(c => (ApprenticeshipProgramme)c).ToList());
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Courses_Are_Cached_Api_Not_Called_And_Retrieved_From_The_Cached(
        Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes.ApprenticeshipProgrammes response,
        [Frozen] Mock<ICache> cache,
        [Frozen] Mock<ITimeProvider> mockTimeProvider,
        [Frozen] Mock<IRecruitOuterApiClient> outerApiClient)
    {
        var dateTime = new DateTime(2025, 2, 1, 6, 0, 0);
        mockTimeProvider.Setup(x => x.NextDay6am).Returns(dateTime);
        cache
            .Setup(x => x.CacheAsideAsync(CacheKeys.ApprenticeshipProgrammes, dateTime, It.IsAny<Func<Task<Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes.ApprenticeshipProgrammes>>>()))
            .ReturnsAsync(response);
        var provider = new ApprenticeshipProgrammeProvider(cache.Object, mockTimeProvider.Object, outerApiClient.Object, Mock.Of<IFeature>());
        
        var actual = await provider.GetApprenticeshipProgrammesAsync(true);

        actual.Should().BeEquivalentTo(response.Data);
        outerApiClient
            .Verify(x => x.Get<GetTrainingProgrammesResponse>(It.IsAny<GetTrainingProgrammesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_ProgrammeId_Is_DummyCourses_And_Retrieved_As_Expected(
        GetTrainingProgrammesResponse apiResponse,
        [Frozen] Mock<ITimeProvider> mockTimeProvider,
        [Frozen] Mock<IRecruitOuterApiClient> outerApiClient)
    {
        var cache = new TestHelpers.TestCache();
        outerApiClient
            .Setup(x => x.Get<GetTrainingProgrammesResponse>(It.IsAny<GetTrainingProgrammesRequest>()))
            .ReturnsAsync(apiResponse);

        var provider = new ApprenticeshipProgrammeProvider(cache, mockTimeProvider.Object, outerApiClient.Object, Mock.Of<IFeature>());

        var actual = await provider.GetApprenticeshipProgrammeAsync("999999");

        actual.Id.Should().Be("999999");
        actual.Title.Should().Be("To be confirmed");
        actual.ApprenticeshipType.Should().Be(TrainingType.Standard);
        actual.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Unknown);
        actual.LastDateStarts.Should().BeAfter(DateTime.UtcNow);
        actual.EffectiveTo.Should().BeAfter(DateTime.UtcNow);
    }
}