using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Providers;

public interface IQualificationsProvider
{
    Task<IList<string>> GetQualificationsAsync();
}