using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.Services.Report;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services.Report;

public class WhenCallingQaReportService
{
    [Test, MoqAutoData]
    public async Task GetReportsAsync_Returns_Reports_From_Outer_Api(
        GetQaReportsApiResponse apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        QaReportService qaReportService)
    {
        outerApiClient
            .Setup(x => x.Get<GetQaReportsApiResponse>(It.IsAny<GetQaReportsApiRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await qaReportService.GetReportsAsync();

        result.Should().BeEquivalentTo(apiResponse);
        outerApiClient.Verify(x => x.Get<GetQaReportsApiResponse>(It.IsAny<GetQaReportsApiRequest>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task GetReportsAsync_Returns_Empty_Response_When_Api_Returns_Null(
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        QaReportService qaReportService)
    {
        outerApiClient
            .Setup(x => x.Get<GetQaReportsApiResponse>(It.IsAny<GetQaReportsApiRequest>()))
            .ReturnsAsync((GetQaReportsApiResponse?)null);

        var result = await qaReportService.GetReportsAsync();

        result.Should().NotBeNull();
        result.Reports.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task CreateQaApplicationsReportAsync_Calls_Outer_Api_With_Correct_Data(
        VacancyUser user,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        QaReportService qaReportService)
    {
        var reportId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var reportName = "Test Report";

        await qaReportService.CreateQaApplicationsReportAsync(reportId, fromDate, toDate, user, reportName);

        outerApiClient.Verify(x => x.Post(
            It.Is<PostCreateQaReportApiRequest>(r =>
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).Id == reportId &&
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).FromDate == fromDate &&
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).ToDate == toDate &&
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).UserId == user.UserId &&
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).CreatedBy == user.Name &&
                ((PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData)r.Data).Name == reportName),
            true), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task GetGenerateQaReportAsync_Returns_Report_Data_From_Outer_Api(
        Guid reportId,
        GetGenerateQaReportApiResponse apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        QaReportService qaReportService)
    {
        outerApiClient
            .Setup(x => x.Get<GetGenerateQaReportApiResponse>(
                It.Is<GetGenerateQaReportApiRequest>(r => r.GetUrl == $"reports/generate/{reportId}")))
            .ReturnsAsync(apiResponse);

        var result = await qaReportService.GetGenerateQaReportAsync(reportId);

        result.Should().BeEquivalentTo(apiResponse);
        outerApiClient.Verify(x => x.Get<GetGenerateQaReportApiResponse>(
            It.Is<GetGenerateQaReportApiRequest>(r => r.GetUrl == $"reports/generate/{reportId}")), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task GetGenerateQaReportAsync_Returns_Empty_Response_When_Api_Returns_Null(
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        QaReportService qaReportService)
    {
        var reportId = Guid.NewGuid();
        outerApiClient
            .Setup(x => x.Get<GetGenerateQaReportApiResponse>(It.IsAny<GetGenerateQaReportApiRequest>()))
            .ReturnsAsync((GetGenerateQaReportApiResponse?)null);

        var result = await qaReportService.GetGenerateQaReportAsync(reportId);

        result.Should().NotBeNull();
        result.QaReports.Should().BeEmpty();
    }
}
