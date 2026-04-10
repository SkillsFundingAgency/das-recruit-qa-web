using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using System;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

internal class MongoDbVacancyRepository(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details)
    : MongoDbCollectionBase(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.Vacancies, details),
        IVacancyRepository
{
    public Task CreateAsync(Vacancy vacancy)
    {
        var collection = GetCollection<Vacancy>();
        return RetryPolicy.ExecuteAsync(_ =>
                collection.InsertOneAsync(vacancy),
            new Context(nameof(CreateAsync)));
    }

    public Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        throw new NotImplementedException();
    }

    public Task<Vacancy> GetVacancyAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        var filter = Builders<Vacancy>.Filter.Eq(v => v.Id, vacancy.Id);
        var collection = GetCollection<Vacancy>();
        await RetryPolicy.ExecuteAsync(async _ =>
                await collection.ReplaceOneAsync(filter, vacancy),
            new Context(nameof(UpdateAsync)));
    }
}