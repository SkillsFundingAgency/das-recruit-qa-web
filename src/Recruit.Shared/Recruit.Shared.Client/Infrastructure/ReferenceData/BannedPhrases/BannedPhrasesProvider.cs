using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Infrastructure.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.BannedPhrases;

public class BannedPhrasesProvider(
    ILogger<BannedPhrasesProvider> logger,
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient,
    ICache cache,
    ITimeProvider timeProvider)
    : IBannedPhrasesProvider
{
    public async Task<IEnumerable<string>> GetBannedPhrasesAsync()
    {
        return await cache.CacheAsideAsync(CacheKeys.BannedPhrases,
            timeProvider.NextDay,
            async () =>
            {
                logger.LogInformation("Attempting to retrieve banned phrases list from reference data.");
                var result = await recruitQaOuterApiVacancyClient.GetBlockedPhrasesAsync();
                if (result != null)
                    return result;
                logger.LogWarning("Unable to retrieve reference data for banned phrases list.");
                return [];
            });
    }
}