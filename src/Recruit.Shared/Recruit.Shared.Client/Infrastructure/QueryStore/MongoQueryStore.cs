using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

internal sealed class MongoQueryStore(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details)
    : MongoDbCollectionBase(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.QueryStore, details),
        IQueryStore, IQueryStoreHouseKeepingService
{
    Task IQueryStore.DeleteAsync<T>(string typeName, string key)
    {
        var collection = GetCollection<T>();

        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.ViewType, typeName)
                     & filterBuilder.Eq(d => d.Id, key);

        return RetryPolicy.ExecuteAsync(_ =>
                collection.DeleteOneAsync(filter),
            new Context(nameof(IQueryStore.DeleteAsync)));
    }

    public async Task<long> DeleteManyLessThanAsync<T, T1>(string typeName, Expression<Func<T, T1>> property, T1 value) where T : QueryProjectionBase
    {
        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.ViewType, typeName)
                     & filterBuilder.Lt(property, value);

        var collection = GetCollection<T>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.DeleteManyAsync(filter),
            new Context(nameof(IQueryStore.DeleteManyLessThanAsync)));

        return result.DeletedCount;
    }

    public async Task<long> DeleteAllAsync<T>(string typeName) where T : QueryProjectionBase
    {
        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.ViewType, typeName);

        var collection = GetCollection<T>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.DeleteManyAsync(filter),
            new Context(nameof(IQueryStore.DeleteManyLessThanAsync)));

        return result.DeletedCount;
    }

    public async Task<IEnumerable<LiveVacancy>> GetAllLiveExpired(DateTime? closingDate)
    {
        var builder = Builders<LiveVacancy>.Filter;
            
        var filter = builder.Lt(identifier => identifier.ClosingDate, closingDate);

        var collection = GetCollection<LiveVacancy>();
            
        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter).Project<LiveVacancy>(GetProjection<LiveVacancy>()).ToListAsync(),
            new Context(nameof(GetAllLiveExpired)));

        return result;
    }

    async Task<T> IQueryStore.GetAsync<T>(string typeName, string key)
    {
        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.ViewType, typeName)
                     & filterBuilder.Eq(d => d.Id, key);

        var collection = GetCollection<T>();
        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter).FirstOrDefaultAsync(),
            new Context(nameof(IQueryStore.GetAsync)));

        return result;
    }

    async Task<T> IQueryStore.GetAsync<T>(string key)
    {
        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.Id, key);

        var collection = GetCollection<T>();
        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter).FirstOrDefaultAsync(),
            new Context(nameof(IQueryStore.GetAsync)));

        return result;
    }

    Task IQueryStore.UpsertAsync<T>(T item)
    {
        var collection = GetCollection<T>();

        var filterBuilder = Builders<T>.Filter;

        var filter = filterBuilder.Eq(d => d.ViewType, item.ViewType)
                     & filterBuilder.Eq(d => d.Id, item.Id);

        return RetryPolicy.ExecuteAsync(_ =>
                collection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true }),
            new Context(nameof(IQueryStore.UpsertAsync)));
    }

    public async Task<IEnumerable<LiveVacancy>> GetAllLiveVacancies(int vacanciesToSkip, int vacanciesToGet)
    {
        var builderFilter = Builders<LiveVacancy>.Filter;
        var filter = builderFilter.Gte(identifier => identifier.ClosingDate, DateTime.UtcNow.Date)
                     & builderFilter.Ne(identifier => identifier.ViewType, "ClosedVacancy");

        var builderSort = Builders<LiveVacancy>.Sort;
        var sort = builderSort.Descending(identifier => identifier.ClosingDate);

        var collection = GetCollection<LiveVacancy>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter).Sort(sort).Skip(vacanciesToSkip).Limit(vacanciesToGet).Project<LiveVacancy>(GetProjection<LiveVacancy>()).ToListAsync(),
            new Context(nameof(GetAllLiveVacancies)));

        return result;
    }

    public async Task<LiveVacancy> GetLiveVacancy(long vacancyReference)
    {
        var builderFilter = Builders<LiveVacancy>.Filter;
        var filter = builderFilter.Eq(identifier => identifier.VacancyReference, vacancyReference)
                     & builderFilter.Gt(identifier => identifier.ClosingDate, DateTime.UtcNow);

        var collection = GetCollection<LiveVacancy>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter).Project<LiveVacancy>(GetProjection<LiveVacancy>()).SingleOrDefaultAsync(),
            new Context(nameof(GetLiveVacancy)));

        return result;
    }
}