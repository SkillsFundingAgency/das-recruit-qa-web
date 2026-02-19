using Recruit.Vacancies.Client.Domain.Entities;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IVacancyReviewRepository
{
    Task UpdateAsync(VacancyReview review);
}