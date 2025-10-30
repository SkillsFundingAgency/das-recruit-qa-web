using System;
using System.IO;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Services.Reports;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Recruit.Vacancies.Client.Infrastructure.Reports;

public class ReportService(
    ILogger<ReportService> logger,
    IReportRepository reportRepository,
    Func<ReportType, IReportStrategy> reportStrategyAccessor,
    ITimeProvider timeProvider,
    ICsvBuilder csvBuilder)
    : IReportService
{
    public async Task GenerateReportAsync(Guid reportId)
    {
        var report = await reportRepository.GetReportAsync(reportId);

        if (report == null)
        {
            logger.LogInformation("Report: {reportId} not found. Ignoring.", reportId);
            return;
        }

        if (report.Status == ReportStatus.Generated)
        {
            logger.LogInformation("Report: {reportId} already generated. Ignoring.", reportId);
            return;
        }

        try
        {
            logger.LogInformation("Report: {reportId} processing.", reportId);
            report.Status = ReportStatus.InProgress;
            report.GenerationStartedOn = timeProvider.Now;

            await reportRepository.UpdateAsync(report);
            logger.LogInformation("Report: {reportId} in progress.", reportId);

            var reportStrategy = reportStrategyAccessor(report.ReportType);
            var reportStrategyResult = await reportStrategy.GetReportDataAsync(report.Parameters);

            report = await reportRepository.GetReportAsync(reportId);
            if (report.Status == ReportStatus.Generated)
            {
                logger.LogInformation("Report: {reportId} already generated in the meantime. Ignoring.", reportId);
                return;
            }
            report.Headers = reportStrategyResult.Headers;
            report.Query = reportStrategyResult.Query;
            report.Data = reportStrategyResult.Data;
            report.Status = ReportStatus.Generated;
            report.GeneratedOn = timeProvider.Now;

            await reportRepository.UpdateAsync(report);
            logger.LogInformation("Report: {reportId} generated.", reportId);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to generate report: {reportId}", report.Id);
            report.Status = ReportStatus.Failed;
            await reportRepository.UpdateAsync(report);
            throw;
        }
    }

    public async Task WriteReportAsCsv(Stream stream, Report report)
    {
        if (report.Status != ReportStatus.Generated)
        {
            throw new Exception($"Cannot download report: {report.Id} as incorrect status: {report.Status}");
        }

        var reportStrategy = reportStrategyAccessor(report.ReportType);
        string reportData;
        if (!string.IsNullOrEmpty(report.Query))
        {
            reportData = await reportStrategy.GetApplicationReviewsRecursiveAsync(report.Query);    
        }
        else
        {
            reportData = report.Data;
        }

        var results = JArray.Parse(reportData);

        csvBuilder.WriteCsvToStream(stream, results, report.Headers, reportStrategy.ResolveFormat);
    }
}