using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client;
[TestFixture]
internal class RecruitQaOuterApiVacancyClientTests
{
    [Test, MoqAutoData]
    public async Task When_Calling_UpsertUserAsync_The_Outer_Api_Is_Called(
        GetQaDashboardApiResponse apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        RecruitQaOuterApiVacancyClient apiVacancyClient)
    {
        outerApiClient.Setup(x => x.Get<GetQaDashboardApiResponse>(It.IsAny<GetQaDashboardApiRequest>())).ReturnsAsync(apiResponse);

        var result = await apiVacancyClient.GetDashboardAsync();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
        outerApiClient.Verify(x => x.Get<GetQaDashboardApiResponse>(It.IsAny<GetQaDashboardApiRequest>()), Times.Once());
    }
}