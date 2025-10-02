using Recruit.Vacancies.Client.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IVacancyRepository
{
    Task CreateAsync(Vacancy vacancy);
    Task UpdateAsync(Vacancy vacancy);
    Task<Vacancy> GetVacancyAsync(Guid id);
    Task<Vacancy> GetVacancyAsync(long vacancyReference);
}