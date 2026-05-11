using System;
using AutoFixture.NUnit4;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
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

    [Test, MoqAutoData]
    public async Task When_Getting_Vacancy_By_Reference_Then_Outer_Api_Is_Called(
        GetVacancyByReferenceApiResponse apiResponse,
        long vacancyReference,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        RecruitQaOuterApiVacancyClient apiVacancyClient)
    {
        outerApiClient.Setup(x => x.Get<GetVacancyByReferenceApiResponse>(It.Is<GetVacancyByReferenceRequest>(r => r.VacancyReference == vacancyReference)))
            .ReturnsAsync(apiResponse);

        var result = await apiVacancyClient.GetVacancyAsync(vacancyReference);

        result.Should().Be(apiResponse.Data);
        outerApiClient.Verify(x => x.Get<GetVacancyByReferenceApiResponse>(It.Is<GetVacancyByReferenceRequest>(r => r.VacancyReference == vacancyReference)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_Updating_Vacancy_From_QaEdits_Then_Outer_Api_Is_Called(
        VacancyQaFieldUpdate vacancyUpdate,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        RecruitQaOuterApiVacancyClient apiVacancyClient)
    {
        await apiVacancyClient.UpdateVacancyFromQaEdits(vacancyUpdate);

        outerApiClient.Verify(x => x.Post(It.Is<PostUpdateVacancyRequest>(r => r.Data == vacancyUpdate)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_Closing_Vacancy_Then_Outer_Api_Is_Called_With_Correct_Data(
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        RecruitQaOuterApiVacancyClient apiVacancyClient)
    {
        var vacancyId = Guid.NewGuid();
        var user = new VacancyUser { Email = "qa@test.com", DfEUserId = "user-1" };

        await apiVacancyClient.CloseVacancy(vacancyId, ClosureReason.Manual);

        outerApiClient.Verify(x => x.Post(It.Is<PostCloseVacancyRequest>(r =>
            r.PostUrl == $"vacancies/close/{vacancyId}")), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_Publishing_Vacancy_Then_Outer_Api_Is_Called(
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        RecruitQaOuterApiVacancyClient apiVacancyClient)
    {
        var vacancyId = Guid.NewGuid();

        await apiVacancyClient.PublishVacancy(vacancyId);

        outerApiClient.Verify(x => x.Post(It.Is<PostPublishVacancyRequest>(r =>
            r.PostUrl == $"vacancies/publish/{vacancyId}")), Times.Once);
    }
}