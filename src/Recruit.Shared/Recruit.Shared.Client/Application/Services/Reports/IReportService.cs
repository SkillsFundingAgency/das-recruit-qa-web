using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services.Reports;

public interface IReportService
{
    void WriteReportAsCsv(Stream stream, List<QaCsvReport> report);
}