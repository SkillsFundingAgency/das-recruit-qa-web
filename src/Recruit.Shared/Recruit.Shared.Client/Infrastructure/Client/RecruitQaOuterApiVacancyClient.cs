using Recruit.Vacancies.Client.Domain.Models;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IRecruitQaOuterApiVacancyClient
{
    Task<QaDashboard> GetDashboardAsync();
}

public class RecruitQaOuterApiVacancyClient(
    IRecruitQaOuterApiClient recruitQaOuterApiClient): IRecruitQaOuterApiVacancyClient
{
    public async Task<QaDashboard> GetDashboardAsync()
    {
        var response = await recruitQaOuterApiClient.Get<GetQaDashboardApiResponse>(new GetQaDashboardApiRequest());

        return QaDashboard.FromApiResponse(response);
    }
}