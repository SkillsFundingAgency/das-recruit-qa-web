using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using static Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests.PostCreateQaReportApiRequest;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public record PostCreateQaReportApiRequest(PostCreateQaReportApiRequestData Payload) : IPostApiRequest
{
    public string PostUrl => "reports/create";
    public object Data { get; set; } = Payload;

    public record PostCreateQaReportApiRequestData
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string UserId { get; init; }
        public required string CreatedBy { get; init; }
        public required DateTime FromDate { get; init; }
        public required DateTime ToDate { get; init; }
    }
}
