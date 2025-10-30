using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreWriter
{
    Task UpdateEmployerVacancyDataAsync(string employerAccountId, IEnumerable<LegalEntity> legalEntities);
    Task UpdateProviderVacancyDataAsync(long ukprn, IEnumerable<EmployerInfo> employers, bool hasAgreement);
    Task UpdateLiveVacancyAsync(LiveVacancy vacancy);
    Task DeleteLiveVacancyAsync(long vacancyReference);
    Task<long> DeleteAllLiveVacancies();
    Task<long> DeleteAllClosedVacancies();
    Task UpdateVacancyApplicationsAsync(VacancyApplications vacancyApplications);
    Task UpdateClosedVacancyAsync(ClosedVacancy closedVacancy);
    Task UpdateBlockedProviders(IEnumerable<BlockedOrganisationSummary> blockedProviders);
    Task UpdateBlockedEmployers(IEnumerable<BlockedOrganisationSummary> blockedEmployers);
}