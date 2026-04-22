using AutoFixture;
using AutoFixture.NUnit4;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Services.Report;
using Recruit.Qa.Web.Orchestrators.Reports;
using Recruit.Qa.Web.ViewModels.Reports;
using Recruit.Qa.Web.ViewModels.Reports.ApplicationsReport;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Qa.Web.Orchestrators.Reports;

public class ApplicationsReportOrchestratorTests
{
    private readonly DateTime _today = new(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
    
    [Test, MoqAutoData]
    public void GetCreateViewModel_Returns_Empty_ViewModel(
        Mock<IQaReportService> qaReportService, 
        Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        var result = applicationsReportOrchestrator.GetCreateViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ApplicationsReportCreateViewModel>();
    }

    [Test, MoqAutoData]
    public void GetCreateViewModel_WithModel_Maps_Fields_From_EditModel(ApplicationsReportCreateEditModel model,
        Mock<IQaReportService> qaReportService, 
        Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        var result = applicationsReportOrchestrator.GetCreateViewModel(model);

        result.DateRange.Should().Be(model.DateRange);
        result.FromDay.Should().Be(model.FromDay);
        result.FromMonth.Should().Be(model.FromMonth);
        result.FromYear.Should().Be(model.FromYear);
        result.ToDay.Should().Be(model.ToDay);
        result.ToMonth.Should().Be(model.ToMonth);
        result.ToYear.Should().Be(model.ToYear);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_Last7Days_Calls_Service_With_Correct_FromDate(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = DateRangeType.Last7Days };

        var reportId = await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        qaReportService.Verify(x => x.CreateQaApplicationsReportAsync(
            reportId,
            _today.AddDays(-7),
            It.IsAny<DateTime>(),
            user,
            It.IsAny<string>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_Last14Days_Calls_Service_With_Correct_FromDate(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = DateRangeType.Last14Days };

        var reportId = await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        qaReportService.Verify(x => x.CreateQaApplicationsReportAsync(
            reportId,
            _today.AddDays(-14),
            It.IsAny<DateTime>(),
            user,
            It.IsAny<string>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_Last30Days_Calls_Service_With_Correct_FromDate(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = DateRangeType.Last30Days };

        var reportId = await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        qaReportService.Verify(x => x.CreateQaApplicationsReportAsync(
            reportId,
            _today.AddDays(-30),
            It.IsAny<DateTime>(),
            user,
            It.IsAny<string>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_Custom_Calls_Service_With_Custom_Dates(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel
        {
            DateRange = DateRangeType.Custom,
            FromDay = "1",
            FromMonth = "3",
            FromYear = "2024",
            ToDay = "31",
            ToMonth = "3",
            ToYear = "2024"
        };

        var reportId = await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        qaReportService.Verify(x => x.CreateQaApplicationsReportAsync(
            reportId,
            It.Is<DateTime>(d => d.Month == 3 && d.Day == 1 && d.Year == 2024),
            It.IsAny<DateTime>(),
            user,
            It.IsAny<string>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_Returns_ReportId_That_Was_Passed_To_Service(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = DateRangeType.Last7Days };
        Guid capturedReportId = Guid.Empty;
        qaReportService
            .Setup(x => x.CreateQaApplicationsReportAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<VacancyUser>(), It.IsAny<string>()))
            .Callback<Guid, DateTime, DateTime, VacancyUser, string>((id, _, _, _, _) => capturedReportId = id);

        var reportId = await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        reportId.Should().Be(capturedReportId);
        reportId.Should().NotBe(Guid.Empty);
    }

    [Test, MoqAutoData]
    public async Task PostCreateViewModelAsync_ToDate_Is_Inclusive_End_Of_Day(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = DateRangeType.Last7Days };
        DateTime capturedToDate = default;
        qaReportService
            .Setup(x => x.CreateQaApplicationsReportAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<VacancyUser>(), It.IsAny<string>()))
            .Callback<Guid, DateTime, DateTime, VacancyUser, string>((_, _, to, _, _) => capturedToDate = to);

        await applicationsReportOrchestrator.PostCreateViewModelAsync(model, user);

        capturedToDate.Should().Be(_today.AddDays(1).AddTicks(-1));
    }

    [Test, MoqAutoData]
    public void PostCreateViewModelAsync_UnknownDateRange_Throws_NotImplementedException(
        VacancyUser user,
        [Frozen] Mock<IQaReportService> qaReportService,
        [Frozen] Mock<ITimeProvider> timeProvider,
        ApplicationsReportOrchestrator applicationsReportOrchestrator)
    {
        timeProvider.Setup(x => x.Today).Returns(_today);
        var model = new ApplicationsReportCreateEditModel { DateRange = (DateRangeType)999 };

        Assert.ThrowsAsync<NotImplementedException>(() => applicationsReportOrchestrator.PostCreateViewModelAsync(model, user));
    }
}
