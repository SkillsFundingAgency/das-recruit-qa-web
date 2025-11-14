using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStore
{
    Task<T> GetAsync<T>(string key) where T : QueryProjectionBase;
    Task<T> GetAsync<T>(string typeName, string key) where T : QueryProjectionBase;

    Task UpsertAsync<T>(T item) where T : QueryProjectionBase;

    Task DeleteAsync<T>(string typeName, string key) where T : QueryProjectionBase;

    Task<long> DeleteManyLessThanAsync<T, T1>(string typeName, Expression<Func<T, T1>> property, T1 value) where T : QueryProjectionBase;

    Task<long> DeleteAllAsync<T>(string typeName) where T : QueryProjectionBase;
}