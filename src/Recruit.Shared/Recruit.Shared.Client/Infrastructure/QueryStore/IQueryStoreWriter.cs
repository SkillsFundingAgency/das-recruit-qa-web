using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreWriter
{
    Task UpdateLiveVacancyAsync(LiveVacancy vacancy);
    Task DeleteLiveVacancyAsync(long vacancyReference);
    Task UpdateClosedVacancyAsync(ClosedVacancy closedVacancy);
    Task UpdateBlockedProviders(IEnumerable<BlockedOrganisationSummary> blockedProviders);
    Task UpdateBlockedEmployers(IEnumerable<BlockedOrganisationSummary> blockedEmployers);
}