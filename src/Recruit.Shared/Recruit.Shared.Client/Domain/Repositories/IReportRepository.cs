using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IReportRepository
{
    Task CreateAsync(Report report);
    Task UpdateAsync(Report report);
    Task<Report> GetReportAsync(Guid reportId);
    Task IncrementReportDownloadCountAsync(Guid reportId);
    Task<List<T>> GetReportsForQaAsync<T>();
}