using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Extensions;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

public class VacancyRepository(
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient) : IVacancyRepository
{
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

    public async Task UpdateVacancyFromQaEdits(VacancyQaFieldUpdate vacancyUpdate)
    {
        await recruitQaOuterApiVacancyClient.UpdateVacancyFromQaEdits(vacancyUpdate);
    }

    public async Task CloseVacancy(Guid messageVacancyId, ClosureReason messageClosureReason)
    {
        await recruitQaOuterApiVacancyClient.CloseVacancy(messageVacancyId, messageClosureReason);
    }

    public async Task PublishVacancy(Guid vacancyId)
    {
        await recruitQaOuterApiVacancyClient.PublishVacancy(vacancyId);
    }
}