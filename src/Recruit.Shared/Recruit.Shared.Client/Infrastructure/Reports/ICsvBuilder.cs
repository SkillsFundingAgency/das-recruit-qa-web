using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Recruit.Vacancies.Client.Infrastructure.Reports;

public interface ICsvBuilder
{
    void WriteCsvToStream(
        Stream stream,
        JArray rows,
        IEnumerable<KeyValuePair<string, string>> headers,
        Func<string, ReportDataType> formatResolver);
}