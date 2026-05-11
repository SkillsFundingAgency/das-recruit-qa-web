using Recruit.Vacancies.Client.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IVacancyRepository
{
    Task<Vacancy> GetVacancyAsync(Guid id);
    Task<Vacancy> GetVacancyAsync(long vacancyReference);
    Task UpdateVacancyFromQaEdits(VacancyQaFieldUpdate vacancyUpdate);
    Task CloseVacancy(Guid messageVacancyId, ClosureReason messageClosureReason);
    Task PublishVacancy(Guid vacancyId);
}