using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;

namespace Recruit.Vacancies.Client.Infrastructure.Repositories;

internal sealed class MongoDbEmployerProfileRepository(
    ILoggerFactory loggerFactory,
    IOptions<MongoDbConnectionDetails> details)
    : MongoDbCollectionBase(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.EmployerProfiles, details),
        IEmployerProfileRepository
{
    public async Task<EmployerProfile> GetAsync(string employerAccountId, string accountLegalEntityPublicHashedId)
    {
        var builder = Builders<EmployerProfile>.Filter;
        var filter = builder.Eq(x => x.Id, EmployerProfile.GetId(employerAccountId, accountLegalEntityPublicHashedId));

        var collection = GetCollection<EmployerProfile>();

        var result = await RetryPolicy.ExecuteAsync(_ => 
                collection.Find(filter).SingleOrDefaultAsync(),
            new Context(nameof(GetAsync)));
            
        return result;
    }
}