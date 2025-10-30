using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.Providers;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.Profanities;

public class ProfanityListProvider(
    IReferenceDataReader referenceDataReader,
    ILogger<ProfanityListProvider> logger,
    ICache cache,
    ITimeProvider timeProvider)
    : IProfanityListProvider
{
    public async Task<IEnumerable<string>> GetProfanityListAsync()
    {
        return await cache.CacheAsideAsync(CacheKeys.Profanities,
            timeProvider.NextDay,
            async () =>
            {
                logger.LogInformation("Attempting to retrieve profanity list from reference data.");
                var result = await referenceDataReader.GetReferenceData<ProfanityList>();
                if (result != null)
                    return result.Profanities;
                logger.LogWarning("Unable to retrieve reference data for profanity list.");
                return new List<string>();
            });
    }
}