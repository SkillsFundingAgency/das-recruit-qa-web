using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Client;

public interface IProviderVacancyClient
{
    Task SetupProviderAsync(long ukprn);
    Task<ProviderEditVacancyInfo> GetProviderEditVacancyInfoAsync(long ukprn);
}