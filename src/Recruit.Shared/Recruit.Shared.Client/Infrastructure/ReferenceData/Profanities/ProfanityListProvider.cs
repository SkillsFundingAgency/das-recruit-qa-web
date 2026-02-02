using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Infrastructure.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.Profanities;

public class ProfanityListProvider(
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient,
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
                var result = await recruitQaOuterApiVacancyClient.GetProfanityListAsync();
                if (result != null)
                    return result;
                logger.LogWarning("Unable to retrieve reference data for profanity list.");
                return [];
            });
    }
}