using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Provider;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.QA;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyAnalytics;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreWriter
{
    Task UpdateEmployerDashboardAsync(string employerAccountId, IEnumerable<VacancySummary> vacancySummaries);
    Task UpdateProviderDashboardAsync(long ukprn, IEnumerable<VacancySummary> vacancySummaries, IEnumerable<ProviderDashboardTransferredVacancy> transferredVacancies);
    Task UpdateEmployerVacancyDataAsync(string employerAccountId, IEnumerable<LegalEntity> legalEntities);
    Task UpdateProviderVacancyDataAsync(long ukprn, IEnumerable<EmployerInfo> employers, bool hasAgreement);
    Task UpdateLiveVacancyAsync(LiveVacancy vacancy);
    Task DeleteLiveVacancyAsync(long vacancyReference);
    Task<long> DeleteAllLiveVacancies();
    Task<long> DeleteAllClosedVacancies();
    Task UpdateVacancyApplicationsAsync(VacancyApplications vacancyApplications);
    Task UpdateQaDashboardAsync(QaDashboard qaDashboard);
    Task UpdateClosedVacancyAsync(ClosedVacancy closedVacancy);
    Task<long> RemoveOldEmployerDashboards(DateTime oldestLastUpdatedDate);
    Task<long> RemoveOldProviderDashboards(DateTime oldestLastUpdatedDate);
    Task UpsertVacancyAnalyticSummaryAsync(VacancyAnalyticsSummary summary);
    Task UpdateBlockedProviders(IEnumerable<BlockedOrganisationSummary> blockedProviders);
    Task UpdateBlockedEmployers(IEnumerable<BlockedOrganisationSummary> blockedEmployers);
    Task UpsertVacancyAnalyticSummaryV2Async(VacancyAnalyticsSummaryV2 summary);
}