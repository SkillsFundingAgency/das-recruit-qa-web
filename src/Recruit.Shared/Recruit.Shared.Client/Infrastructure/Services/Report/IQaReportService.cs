using System;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Report;

public interface IQaReportService
{
    Task<GetQaReportsApiResponse> GetReportsAsync();
    Task CreateQaApplicationsReportAsync(Guid reportId, DateTime fromDate, DateTime toDate, VacancyUser user, string reportName);
    Task<GetGenerateQaReportApiResponse> GetGenerateQaReportAsync(Guid reportId);
}
