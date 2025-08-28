using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Providers;

public interface IProfanityListProvider
{
    Task<IEnumerable<string>> GetProfanityListAsync();
}