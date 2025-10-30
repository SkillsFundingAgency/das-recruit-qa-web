using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Recruit.Vacancies.Client.Application.Exceptions;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Exceptions;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

internal class MongoDbVacancyRepository(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details)
    : MongoDbCollectionBase(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.Vacancies, details),
        IVacancyRepository, IVacancyQuery
{
    private const string VacancyStatusFieldName = "status";

    public Task CreateAsync(Vacancy vacancy)
    {
        var collection = GetCollection<Vacancy>();
        return RetryPolicy.ExecuteAsync(_ =>
                collection.InsertOneAsync(vacancy),
            new Context(nameof(CreateAsync)));
    }

    public async Task<Vacancy> GetVacancyAsync(long vacancyReference)
    {
        var vacancy = await FindVacancy(v => v.VacancyReference, vacancyReference);

        if (vacancy == null)
            throw new VacancyNotFoundException(string.Format(ExceptionMessages.VacancyWithReferenceNotFound, vacancyReference));

        return vacancy;
    }

    public async Task<Vacancy> GetVacancyAsync(Guid id)
    {
        var vacancy = await FindVacancy(v => v.Id, id);

        if (vacancy == null)
            throw new VacancyNotFoundException(string.Format(ExceptionMessages.VacancyWithIdNotFound, id));

        return vacancy;
    }

    private async Task<Vacancy> FindVacancy<TField>(Expression<Func<Vacancy, TField>> expression, TField value)
    {
        var filter = Builders<Vacancy>.Filter.Eq(expression, value);
        var collection = GetCollection<Vacancy>();

        var result = await RetryPolicy.ExecuteAsync(async _ => await collection.Find(filter).SingleOrDefaultAsync(),
            new Context(nameof(FindVacancy)));

        return result;
    }

    public async Task<IEnumerable<T>> GetVacanciesByStatusAsync<T>(VacancyStatus status)
    {
        var filter = Builders<T>.Filter.Eq(VacancyStatusFieldName, status.ToString());

        var collection = GetCollection<T>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter)
                    .Project<T>(GetProjection<T>())
                    .ToListAsync(),
            new Context(nameof(GetVacanciesByStatusAsync)));

        return result;
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