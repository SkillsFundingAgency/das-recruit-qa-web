using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Models;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Encoding;
using VacancyDto = Recruit.Vacancies.Client.Domain.Models.VacancyDto;

namespace Recruit.Vacancies.Client.Infrastructure.Client;

public interface IRecruitQaOuterApiVacancyClient
{
    Task<QaDashboard> GetDashboardAsync();
    Task<List<string>> GetProfanityListAsync();
    Task<List<string>> GetBlockedPhrasesAsync();
    Task<Provider> GetProviderAsync(long ukprn);
    Task<VacancyDto> GetVacancyAsync(Guid id);
    Task<VacancyDto> GetVacancyAsync(long vacancyReference);
    Task UpdateAsync(Vacancy vacancy);
}

public class RecruitQaOuterApiVacancyClient(IRecruitQaOuterApiClient recruitQaOuterApiClient, IEncodingService encodingService): IRecruitQaOuterApiVacancyClient
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

    public async Task<VacancyDto> GetVacancyAsync(Guid id)
    {
        var response = await recruitQaOuterApiClient.Get<GetVacancyByIdApiResponse>(new GetVacancyByIdRequest(id));
        return response?.Data;
    }

    public async Task<VacancyDto> GetVacancyAsync(long vacancyReference)
    {
        var response = await recruitQaOuterApiClient.Get<GetVacancyByReferenceApiResponse>(new GetVacancyByReferenceRequest(vacancyReference));
        return response?.Data;
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {   
        await recruitQaOuterApiClient.Post(new PostVacancyRequest(vacancy.Id, Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests.VacancyDto.From(vacancy, encodingService)));;
    }
}