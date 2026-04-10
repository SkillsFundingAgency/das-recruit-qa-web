using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreReader
{
    Task<ClosedVacancy> GetClosedVacancy(long vacancyReference);
}