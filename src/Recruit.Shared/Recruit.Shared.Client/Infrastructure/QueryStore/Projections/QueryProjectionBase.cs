using System;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;

public class QueryProjectionBase
{
    public QueryProjectionBase(string viewType)
    {
        ViewType = viewType;
    }

    public string Id { get; set; }
    public string ViewType { get; set; }
    public DateTime LastUpdated { get; set; }
}