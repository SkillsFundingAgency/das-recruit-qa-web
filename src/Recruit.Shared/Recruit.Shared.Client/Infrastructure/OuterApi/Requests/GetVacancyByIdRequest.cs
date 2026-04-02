using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using System;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public sealed record GetVacancyByIdRequest(Guid Id) : IGetApiRequest
{
    public string GetUrl => $"vacancies/{Id}";
}