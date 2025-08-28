using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Projections;

public interface IQaDashboardProjectionService
{
    Task RebuildQaDashboardAsync();
}