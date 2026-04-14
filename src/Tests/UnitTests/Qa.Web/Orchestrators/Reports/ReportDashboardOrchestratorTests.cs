using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Exceptions;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.Services.Report;
using Recruit.Qa.Web.Orchestrators.Reports;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Qa.Web.Orchestrators.Reports;

public class ReportDashboardOrchestratorTests
{
    [Test, MoqAutoData]
    public async Task GetDashboardViewModel_Returns_Reports_From_Service(
        List<QaReportSummary> reports,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        qaReportService
            .Setup(x => x.GetReportsAsync())
            .ReturnsAsync(new GetQaReportsApiResponse { Reports = reports });
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        var result = await sut.GetDashboardViewModel();

        result.Reports.Should().HaveCount(reports.Count);
        result.Reports.Select(r => r.ReportId).Should().BeEquivalentTo(reports.Select(r => r.Id));
    }

    [Test, MoqAutoData]
    public async Task GetDashboardViewModel_Returns_Reports_Ordered_By_CreatedDate_Descending(
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        var baseDate = DateTime.UtcNow;
        var reports = new List<QaReportSummary>
        {
            new() { Id = Guid.NewGuid(), Name = "oldest", CreatedDate = baseDate.AddDays(-2) },
            new() { Id = Guid.NewGuid(), Name = "newest", CreatedDate = baseDate },
            new() { Id = Guid.NewGuid(), Name = "middle", CreatedDate = baseDate.AddDays(-1) }
        };
        qaReportService
            .Setup(x => x.GetReportsAsync())
            .ReturnsAsync(new GetQaReportsApiResponse { Reports = reports });
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        var result = await sut.GetDashboardViewModel();

        result.Reports.First().ReportName.Should().Be("newest");
        result.Reports.Last().ReportName.Should().Be("oldest");
    }

    [Test, MoqAutoData]
    public async Task GetDashboardViewModel_Maps_Report_Fields_Correctly(
        QaReportSummary report,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        qaReportService
            .Setup(x => x.GetReportsAsync())
            .ReturnsAsync(new GetQaReportsApiResponse { Reports = [report] });
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        var result = await sut.GetDashboardViewModel();

        var row = result.Reports.Single();
        row.ReportId.Should().Be(report.Id);
        row.ReportName.Should().Be(report.Name);
        row.DownloadCount.Should().Be(report.DownloadCount);
        row.CreatedBy.Should().Be(report.CreatedBy);
        row.IsProcessing.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task GetDashboardViewModel_ProcessingCount_Is_Zero(
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        qaReportService
            .Setup(x => x.GetReportsAsync())
            .ReturnsAsync(new GetQaReportsApiResponse { Reports = [] });
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        var result = await sut.GetDashboardViewModel();

        result.ProcessingCount.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task GetDownloadCsvAsync_Returns_DownloadViewModel_With_Stream_And_ReportName(
        Guid reportId,
        List<QaReport> qaReports,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        var apiResponse = new GetGenerateQaReportApiResponse { ReportName = "Test Report", QaReports = qaReports };
        qaReportService
            .Setup(x => x.GetGenerateQaReportAsync(reportId))
            .ReturnsAsync(apiResponse);
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        var result = await sut.GetDownloadCsvAsync(reportId);

        result.ReportName.Should().Be("Test Report");
        result.Content.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetDownloadCsvAsync_Calls_WriteReportAsCsv_On_Client(
        Guid reportId,
        List<QaReport> qaReports,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        var apiResponse = new GetGenerateQaReportApiResponse { ReportName = "Test Report", QaReports = qaReports };
        qaReportService
            .Setup(x => x.GetGenerateQaReportAsync(reportId))
            .ReturnsAsync(apiResponse);
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        await sut.GetDownloadCsvAsync(reportId);

        client.Verify(x => x.WriteReportAsCsv(It.IsAny<Stream>(), It.IsAny<List<QaCsvReport>>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task GetDownloadCsvAsync_Throws_ReportNotFoundException_When_ReportName_Is_Null(
        Guid reportId,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<IQaVacancyClient> client,
        [Frozen] Mock<ILogger<ReportDashboardOrchestrator>> logger)
    {
        qaReportService
            .Setup(x => x.GetGenerateQaReportAsync(reportId))
            .ReturnsAsync(new GetGenerateQaReportApiResponse { ReportName = null });
        var sut = new ReportDashboardOrchestrator(logger.Object, qaReportService.Object, client.Object);

        Assert.ThrowsAsync<ReportNotFoundException>(() => sut.GetDownloadCsvAsync(reportId));
    }
}
