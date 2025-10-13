using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.BlockedOrganisations;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreReader
{
    Task<ProviderEditVacancyInfo> GetProviderVacancyDataAsync(long ukprn);
    Task<BlockedProviderOrganisations> GetBlockedProvidersAsync();
    Task<ClosedVacancy> GetClosedVacancy(long vacancyReference);
}