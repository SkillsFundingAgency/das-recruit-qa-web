using Recruit.Vacancies.Client.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IVacancyQuery
{
    Task<IEnumerable<T>> GetVacanciesByStatusAsync<T>(VacancyStatus status);

    Task<Vacancy> GetVacancyAsync(Guid id);
}