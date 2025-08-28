using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections;

namespace Recruit.Vacancies.Client.Infrastructure.Services.VacancySummariesProvider;

internal static class VacancyDashboardMapper
{
    internal static VacancyStatusDashboard MapFromVacancyDashboardSummaryResponseDto(VacancyDashboardAggQueryResponseDto src)
    {
        return new VacancyStatusDashboard
        {
            Status = src.Id.Status,
            StatusCount = src.StatusCount,
            ClosingSoon = src.Id.ClosingSoon,
        };
    }
}