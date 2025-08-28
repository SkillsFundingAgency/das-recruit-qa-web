using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IEmployerProfileRepository
{
    Task CreateAsync(EmployerProfile profile);
    Task<IList<EmployerProfile>> GetEmployerProfilesForEmployerAsync(string employerAccountId);
    Task<EmployerProfile> GetAsync(string employerAccountId, string accountLegalEntityPublicHashedId);
    Task UpdateAsync(EmployerProfile profile);
}