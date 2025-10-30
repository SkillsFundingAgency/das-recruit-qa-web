using System.Collections.Generic;
using System.Linq;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Provider;

public class ProviderDashboard() : QueryProjectionBase(QueryViewType.ProviderDashboard.TypeName)
{
    public IEnumerable<VacancySummary.VacancySummary> Vacancies { get; set; }
    public int? TotalVacancies { get; set; } = null;

    public IEnumerable<ProviderDashboardTransferredVacancy> TransferredVacancies { get; set; }

    public IEnumerable<VacancySummary.VacancySummary> CloneableVacancies => Vacancies.Where(
        x => x.Status == VacancyStatus.Live ||
             x.Status == VacancyStatus.Closed ||
             x.Status == VacancyStatus.Submitted);
}