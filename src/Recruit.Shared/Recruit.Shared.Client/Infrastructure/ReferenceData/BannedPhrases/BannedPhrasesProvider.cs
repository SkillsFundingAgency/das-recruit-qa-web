using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.Providers;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.BannedPhrases;

public class BannedPhrasesProvider(
    ILogger<BannedPhrasesProvider> logger,
    IReferenceDataReader referenceDataReader,
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
                var result = await referenceDataReader.GetReferenceData<BannedPhraseList>();
                if (result != null)
                    return result.BannedPhrases;
                logger.LogWarning("Unable to retrieve reference data for banned phrases list.");
                return new List<string>();
            });
    }
}