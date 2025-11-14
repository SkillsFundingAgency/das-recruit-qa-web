using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.Reports;

public class ReportStrategyResult(IEnumerable<KeyValuePair<string, string>> headers, string data, string query)
{
    public IEnumerable<KeyValuePair<string, string>> Headers { get; set; } = headers;
    public string Data { get; set; } = data;
    public string Query { get; set; } = query;
}