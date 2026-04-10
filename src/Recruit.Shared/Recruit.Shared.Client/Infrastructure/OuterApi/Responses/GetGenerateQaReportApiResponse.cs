using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public record GetGenerateQaReportApiResponse
{
    public List<QaReport> QaReports { get; init; } = [];
    public string ReportName { get; init; } = "";
}
