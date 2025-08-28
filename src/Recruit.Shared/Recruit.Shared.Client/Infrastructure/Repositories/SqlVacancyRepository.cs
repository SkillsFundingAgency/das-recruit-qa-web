using System;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Client;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

// TODO: Proxies calls to the new outer api endpoints - this class should go once we have migrated vacancies over to SQL
public class SqlVacancyRepository(IOuterApiVacancyClient outerApiVacancyClient): IVacancyRepository
{
    public async Task CreateAsync(Vacancy vacancy)
    {
        await outerApiVacancyClient.CreateAsync(vacancy);
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        await outerApiVacancyClient.UpdateAsync(vacancy);
    }

    public Task<Vacancy> GetVacancyAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        throw new NotImplementedException();
    }
}