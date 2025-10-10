using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.EmployerAccount;

public interface IEmployerAccountProvider
{
    Task<IEnumerable<AccountLegalEntity>> GetLegalEntitiesConnectedToAccountAsync(string hashedAccountId);
}