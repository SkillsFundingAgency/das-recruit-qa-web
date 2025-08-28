using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IProviderVacancyClient
{
    Task SetupProviderAsync(long ukprn);
    Task<ProviderEditVacancyInfo> GetProviderEditVacancyInfoAsync(long ukprn);
}