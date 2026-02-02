using Recruit.Vacancies.Client.Domain.Entities;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Providers;

public interface IApprenticeshipProgrammeProvider
{
    Task<IApprenticeshipProgramme> GetApprenticeshipProgrammeAsync(string programmeId);
}