using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using SFA.DAS.Encoding;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IOuterApiVacancyClient
{
    Task CreateAsync(Vacancy vacancy);
    Task UpdateAsync(Vacancy vacancy);
}

public class OuterApiVacancyClient(
    IEncodingService encodingService,
    IOuterApiClient apimRecruitClient): IOuterApiVacancyClient
{
    public async Task CreateAsync(Vacancy vacancy)
    {
        // TODO: we'll want the returned data here at some point
        await apimRecruitClient.Post(new PostVacancyRequest(vacancy.Id, VacancyDto.From(vacancy, encodingService)));
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        // TODO: we'll want the returned data here at some point
        await apimRecruitClient.Post(new PostVacancyRequest(vacancy.Id, VacancyDto.From(vacancy, encodingService)));
    }
}