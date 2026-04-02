using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Extensions;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

// TODO: Proxies calls to the new outer api endpoints - this class should go once we have migrated vacancies over to SQL
public class SqlVacancyRepository(IRecruitOuterApiVacancyClient recruitOuterApiVacancyClient,
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient) : IVacancyRepository
{
    public async Task CreateAsync(Vacancy vacancy)
    {
        await recruitOuterApiVacancyClient.CreateAsync(vacancy);
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        await recruitOuterApiVacancyClient.UpdateAsync(vacancy);
    }

    public async Task<Vacancy> GetVacancyAsync(Guid id)
    {
        var vacancy = await recruitQaOuterApiVacancyClient.GetVacancyAsync(id);
        return vacancy?.ToVacancy();
    }

    public async Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        var vacancy = await recruitQaOuterApiVacancyClient.GetVacancyAsync(vacancyReference);
        return vacancy?.ToVacancy();
    }
}