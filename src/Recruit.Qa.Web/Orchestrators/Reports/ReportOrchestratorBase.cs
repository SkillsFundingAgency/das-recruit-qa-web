using System;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Exceptions;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Recruit.Qa.Web.Orchestrators.Reports;

public abstract class ReportOrchestratorBase(ILogger logger, IQaVacancyClient client)
{
    protected async Task<Report> GetReportAsync(Guid reportId)
    {
        var report = await client.GetReportAsync(reportId);

        if (report == null)
        {
            logger.LogInformation("Cannot find report: {reportId}", reportId);
            throw new ReportNotFoundException($"Cannot find report: {reportId}");
        }

        if (report.Owner.OwnerType == ReportOwnerType.Qa)
        {
            return report;
        }

        logger.LogWarning("QA user does not have access to report: {reportId}", reportId);
        throw new AuthorisationException($"QA user does not have access to report: {reportId}");
    }
}