using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IApplicationReviewQuery
{
    Task<List<T>> GetForVacancyAsync<T>(long vacancyReference);
}