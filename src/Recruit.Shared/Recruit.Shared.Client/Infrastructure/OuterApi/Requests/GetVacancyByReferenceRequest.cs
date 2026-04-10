using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public sealed record GetVacancyByReferenceRequest(long VacancyReference) : IGetApiRequest
{
    public string GetUrl => $"vacancies/by/ref/{VacancyReference}";
}