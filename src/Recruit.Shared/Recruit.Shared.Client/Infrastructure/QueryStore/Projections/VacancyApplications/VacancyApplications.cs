using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancyApplications;

public class VacancyApplications() : QueryProjectionBase(QueryViewType.VacancyApplications.TypeName)
{
    public long VacancyReference { get; set; }
    public List<VacancyApplication> Applications { get; set; }
}