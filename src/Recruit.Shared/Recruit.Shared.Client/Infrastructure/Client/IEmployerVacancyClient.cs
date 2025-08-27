using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Client;

public interface IEmployerVacancyClient
{
    Task<EmployerEditVacancyInfo> GetEditVacancyInfoAsync(string employerAccountId);
    Task<IEnumerable<LegalEntity>> GetEmployerLegalEntitiesAsync(string employerAccountId);
    Task SetupEmployerAsync(string employerAccountId);
}