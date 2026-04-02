using Recruit.Vacancies.Client.Domain.Models;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public sealed record GetVacancyByIdApiResponse
{
    public VacancyDto Data { get; init; }
}