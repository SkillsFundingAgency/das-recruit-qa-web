using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Cache;

public interface ICache
{
    Task<T> CacheAsideAsync<T>(string key, DateTime absoluteExpiration, Func<Task<T>> objectToCache);
}