using System;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

public class MigrationVacancyRepository([FromKeyedServices("sql")] IVacancyRepository sqlRepository): IVacancyRepository
{
    public async Task UpdateAsync(Vacancy vacancy)
    {
        await sqlRepository.UpdateAsync(vacancy);
    }

    public Task<Vacancy> GetVacancyAsync(Guid id)
    {
        return sqlRepository.GetVacancyAsync(id);
    }

    public Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        return sqlRepository.GetVacancyAsync(vacancyReference);
    }
}