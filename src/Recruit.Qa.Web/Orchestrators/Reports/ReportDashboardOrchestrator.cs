using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Recruit.Qa.Web.ViewModels.Reports.ReportDashboard;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Exceptions;
using Recruit.Vacancies.Client.Infrastructure.Services.Report;

namespace Recruit.Qa.Web.Orchestrators.Reports;

public class ReportDashboardOrchestrator(ILogger<ReportDashboardOrchestrator> logger, IQaReportService qaReportService, IQaVacancyClient client)
{

    public async Task<ReportsDashboardViewModel> GetDashboardViewModel()
    {
        var reportsResponse = await qaReportService.GetReportsAsync();
        var reports = reportsResponse.Reports;
        
        var vm = new ReportsDashboardViewModel
        {
            ProcessingCount = 0,
            Reports = reports
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => new ReportRowViewModel
                {
                    ReportId = r.Id,
                    ReportName = r.Name,
                    DownloadCount = r.DownloadCount,
                    CreatedDate = r.CreatedDate.ToUkTime().AsGdsDateTime(),
                    CreatedBy = r.CreatedBy,
                    Status = ReportStatus.Generated,
                    IsProcessing = false
                })
        };

        return vm;
    }

    public async Task<ReportDownloadViewModel> GetDownloadCsvAsync(Guid reportId)
    {
        var report = await qaReportService.GetGenerateQaReportAsync(reportId);
        if (report.ReportName == null)
        {
            logger.LogInformation("Cannot find report: {reportId}", reportId);
            throw new ReportNotFoundException($"Cannot find report: {reportId}");
        }

        var stream = new MemoryStream();
            
        client.WriteReportAsCsv(stream, report.QaReports.Select(c=>(QaCsvReport)c).ToList());

        return new ReportDownloadViewModel
        {
            Content = stream,
            ReportName = report.ReportName
        };
    }
}