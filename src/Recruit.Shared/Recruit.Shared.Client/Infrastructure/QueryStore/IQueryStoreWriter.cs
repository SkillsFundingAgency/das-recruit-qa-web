using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreWriter
{
    Task UpdateLiveVacancyAsync(LiveVacancy vacancy);
    Task DeleteLiveVacancyAsync(long vacancyReference);
    Task UpdateClosedVacancyAsync(ClosedVacancy closedVacancy);
}