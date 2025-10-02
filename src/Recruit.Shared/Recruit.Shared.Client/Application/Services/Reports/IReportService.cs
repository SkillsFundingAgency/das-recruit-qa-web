using System;
using System.IO;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services.Reports;

public interface IReportService
{
    Task GenerateReportAsync(Guid reportId);
    Task WriteReportAsCsv(Stream stream, Report report);
}