using Recruit.Vacancies.Client.Domain.Models;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IRecruitQaOuterApiVacancyClient
{
    Task<QaDashboard> GetDashboardAsync();
    Task<List<string>> GetProfanityListAsync();
    Task<List<string>> GetBlockedPhrasesAsync();
    Task<Provider> GetProviderAsync(long ukprn);
}

public class RecruitQaOuterApiVacancyClient(IRecruitQaOuterApiClient recruitQaOuterApiClient): IRecruitQaOuterApiVacancyClient
{
    public async Task<QaDashboard> GetDashboardAsync()
    {
        var response = await recruitQaOuterApiClient.Get<GetQaDashboardApiResponse>(new GetQaDashboardApiRequest());

        return QaDashboard.FromApiResponse(response);
    }

    public async Task<List<string>> GetProfanityListAsync()
    {
        return await recruitQaOuterApiClient.Get<List<string>>(new GetProfanityListApiRequest());
    }

    public async Task<List<string>> GetBlockedPhrasesAsync()
    {
        return await recruitQaOuterApiClient.Get<List<string>>(new GetBannedPhrasesListApiRequest());
    }

    public async Task<Provider> GetProviderAsync(long ukprn)
    {
        return await recruitQaOuterApiClient.Get<GetProviderApiResponse>(new GetProviderRequest(ukprn));
    }
}