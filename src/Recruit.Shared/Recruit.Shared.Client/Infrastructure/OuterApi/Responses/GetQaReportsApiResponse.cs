using System;
using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public record GetQaReportsApiResponse
{
    public List<QaReportSummary> Reports { get; init; } = [];
}

public record QaReportSummary
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int DownloadCount { get; init; }
}
