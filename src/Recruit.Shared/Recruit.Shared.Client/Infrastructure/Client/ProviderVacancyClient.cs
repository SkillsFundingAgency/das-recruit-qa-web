using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public partial class VacancyClient : IProviderVacancyClient
{
    public Task<ProviderEditVacancyInfo> GetProviderEditVacancyInfoAsync(long ukprn)
    {
        return reader.GetProviderVacancyDataAsync(ukprn);
    }

    public Task SetupProviderAsync(long ukprn)
    {
        var command = new SetupProviderCommand(ukprn);

        return messaging.SendCommandAsync(command);
    }
}