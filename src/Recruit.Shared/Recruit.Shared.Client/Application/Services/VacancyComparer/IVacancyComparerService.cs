using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services.VacancyComparer;

public interface IVacancyComparerService
{
    VacancyComparerResult Compare(Vacancy a, Vacancy b);
}