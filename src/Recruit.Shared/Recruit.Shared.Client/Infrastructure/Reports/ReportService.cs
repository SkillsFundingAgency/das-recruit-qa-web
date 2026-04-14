using System;
using System.Collections.Generic;
using System.IO;
using Recruit.Vacancies.Client.Application.Services.Reports;
using Recruit.Vacancies.Client.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Recruit.Vacancies.Client.Infrastructure.Reports;

public class ReportService(ICsvBuilder csvBuilder)
    : IReportService
{
    public void WriteReportAsCsv(Stream stream, List<QaCsvReport> report)
    {
        var results = JArray.Parse(JsonConvert.SerializeObject(report));

        csvBuilder.WriteCsvToStream(stream, results, new List<KeyValuePair<string, string>>{new("Date", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))}, ResolveFormat);
    }
    
    private ReportDataType ResolveFormat(string fieldName)
    {
        if (fieldName.Equals("Referred Fields", StringComparison.CurrentCultureIgnoreCase))
        {
            return ReportDataType.ArrayType;
        }

        if (fieldName.Equals("SLA deadline", StringComparison.CurrentCultureIgnoreCase)
            || fieldName.Equals("Date submitted", StringComparison.CurrentCultureIgnoreCase)
            || fieldName.Equals("Review completed", StringComparison.CurrentCultureIgnoreCase)
            || fieldName.Equals("Review started", StringComparison.CurrentCultureIgnoreCase))
        {
            return ReportDataType.DateTimeType;
        }
            
        return ReportDataType.StringType;
    }
}