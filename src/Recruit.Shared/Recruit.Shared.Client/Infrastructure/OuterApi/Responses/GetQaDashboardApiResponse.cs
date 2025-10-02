namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
public record GetQaDashboardApiResponse
{
    public int TotalVacanciesForReview { get; init; } = 0;
    public int TotalVacanciesBrokenSla { get; init; } = 0;
    public int TotalVacanciesResubmitted { get; init; } = 0;
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; init; } = 0;
}