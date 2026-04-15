using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Report;

public class QaReportService(
    ILogger<QaReportService> logger,
    IRecruitQaOuterApiClient outerApiClient) : IQaReportService
{
    public async Task<GetQaReportsApiResponse> GetReportsAsync()
    {
        logger.LogTrace("Getting QA reports from Outer Api");

        var retryPolicy = PollyRetryPolicy.GetPolicy();

        var result = await retryPolicy.Execute(_ =>
                outerApiClient.Get<GetQaReportsApiResponse>(new GetQaReportsApiRequest()),
            new Dictionary<string, object>
            {
                { "apiCall", "Reports" }
            });

        return result ?? new GetQaReportsApiResponse();
    }

    public async Task CreateQaApplicationsReportAsync(Guid reportId, DateTime fromDate, DateTime toDate, VacancyUser user, string reportName)
    {
        logger.LogTrace("Posting QA Report to Outer Api");

        var retryPolicy = PollyRetryPolicy.GetPolicy();

        await retryPolicy.Execute(_ =>
                outerApiClient.Post(new PostCreateQaReportApiRequest(new PostCreateQaReportApiRequest.PostCreateQaReportApiRequestData
                {
                    Id = reportId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    CreatedBy = user.Name,
                    UserId = user.UserId,
                    Name = reportName
                })),
            new Dictionary<string, object>
            {
                { "apiCall", "Reports" }
            });
    }

    public async Task<GetGenerateQaReportApiResponse> GetGenerateQaReportAsync(Guid reportId)
    {
        logger.LogTrace("Getting QA Report Data from Outer Api");

        var retryPolicy = PollyRetryPolicy.GetPolicy();

        var result = await retryPolicy.Execute(_ =>
                outerApiClient.Get<GetGenerateQaReportApiResponse>(new GetGenerateQaReportApiRequest(reportId)),
            new Dictionary<string, object>
            {
                { "apiCall", "Reports" }
            });

        return result ?? new GetGenerateQaReportApiResponse();
    }
}
