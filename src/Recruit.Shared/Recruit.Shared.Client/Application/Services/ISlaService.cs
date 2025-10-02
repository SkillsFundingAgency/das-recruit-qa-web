using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Services;

public interface ISlaService
{
    Task<DateTime> GetSlaDeadlineAsync(DateTime date);
}