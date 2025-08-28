using System.Collections.Generic;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public class GetVacanciesWithMetricsResponse
{
    [JsonProperty("vacancyMetrics")]
    public List<GetVacancyMetricResponse> VacancyMetrics { get; set; }
}