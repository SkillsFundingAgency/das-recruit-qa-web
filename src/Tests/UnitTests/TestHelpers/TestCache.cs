using Recruit.Vacancies.Client.Application.Cache;

namespace Recruit.Qa.Vacancies.Client.UnitTests.TestHelpers;

public class TestCache : ICache
{
    public Task<T> CacheAsideAsync<T>(string key, DateTime absoluteExpiration, Func<Task<T>> objectToCache)
    {
        return objectToCache();
    }
}