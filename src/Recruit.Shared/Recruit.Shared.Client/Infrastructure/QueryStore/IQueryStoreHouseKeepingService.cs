using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore;

public interface IQueryStoreHouseKeepingService
{
    Task<List<T>> GetStaleDocumentsAsync<T>(string typeName, DateTime documentsNotAccessedSinceDate) where T : QueryProjectionBase;

    Task<long> DeleteStaleDocumentsAsync<T>(string typeName, IEnumerable<string> documentIds) where T : QueryProjectionBase;

}