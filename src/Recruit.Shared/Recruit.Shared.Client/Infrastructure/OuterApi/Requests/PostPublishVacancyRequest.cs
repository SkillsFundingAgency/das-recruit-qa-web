using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public sealed record PostPublishVacancyRequest(Guid VacancyId): IPostApiRequest
{
    public string PostUrl => $"vacancies/{VacancyId}/publish";
    public object Data { get; set; } = null;
}