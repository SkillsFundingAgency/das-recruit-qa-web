using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IEmployerProfileRepository
{
    Task<EmployerProfile> GetAsync(string employerAccountId, string accountLegalEntityPublicHashedId);
}