using System;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;

public class QueryProjectionBase(string viewType)
{
    public string Id { get; set; }
    public string ViewType { get; set; } = viewType;
    public DateTime LastUpdated { get; set; }
}