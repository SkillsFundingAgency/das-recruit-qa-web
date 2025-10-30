using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

internal class MongoDbReportRepository(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details)
    : MongoDbCollectionBase(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.Reports, details),
        IReportRepository
{
    private const string OwnerTypeFieldName = "owner.ownerType";

    public Task CreateAsync(Report report)
    {
        var collection = GetCollection<Report>();
        return RetryPolicy.ExecuteAsync(_ => 
                collection.InsertOneAsync(report), 
            new Context(nameof(CreateAsync)));
    }

    public async Task UpdateAsync(Report report)
    {
        var filter = Builders<Report>.Filter.Eq(v => v.Id, report.Id);
        var collection = GetCollection<Report>();
        await RetryPolicy.ExecuteAsync(_ =>
                collection.ReplaceOneAsync(filter, report),
            new Context(nameof(UpdateAsync)));
    }

   
    public async Task<List<T>> GetReportsForQaAsync<T>()
    {
        var builder = Builders<T>.Filter;
        var filter = builder.Eq(OwnerTypeFieldName, ReportOwnerType.Qa.ToString());

        var collection = GetCollection<T>();

        var result = await RetryPolicy.ExecuteAsync(_ =>
                collection.Find(filter)
                    .Project<T>(GetProjection<T>())
                    .ToListAsync(),
            new Context(nameof(GetReportsForQaAsync)));

        return result;
    }

    public async Task<Report> GetReportAsync(Guid reportId)
    {
        var filter = Builders<Report>.Filter.Eq(r => r.Id, reportId);
        var collection = GetCollection<Report>();

        var result = await RetryPolicy.ExecuteAsync(async _ =>
                await collection.Find(filter).SingleOrDefaultAsync(),
            new Context(nameof(GetReportAsync)));

        return result;
    }

    public Task IncrementReportDownloadCountAsync(Guid reportId)
    {
        var filter = Builders<Report>.Filter.Eq(r => r.Id, reportId);
        var update = new UpdateDefinitionBuilder<Report>().Inc(r => r.DownloadCount, 1);
        var collection = GetCollection<Report>();

        return RetryPolicy.ExecuteAsync(async _ =>
                await collection.FindOneAndUpdateAsync(filter, update),
            new Context(nameof(IncrementReportDownloadCountAsync)));
    }
}