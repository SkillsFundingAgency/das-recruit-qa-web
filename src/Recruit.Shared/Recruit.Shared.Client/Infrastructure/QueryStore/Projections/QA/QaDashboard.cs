namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.QA;

public class QaDashboard() : QueryProjectionBase(QueryViewType.QaDashboard.TypeName)
{
    public int TotalVacanciesForReview { get; set; }
    public int TotalVacanciesBrokenSla { get; set; }
    public int TotalVacanciesResubmitted { get; set; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; set; }
}