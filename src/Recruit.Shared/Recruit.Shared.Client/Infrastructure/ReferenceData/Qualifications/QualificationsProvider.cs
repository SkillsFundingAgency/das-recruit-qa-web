using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.Qualifications;

public class QualificationsProvider : IQualificationsProvider
{
    public Task<IList<string>> GetQualificationsAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            "GCSE",
            "A Level",
            "T Level",
            "BTEC",
            "Degree",
            "Other"
        });
    }
}