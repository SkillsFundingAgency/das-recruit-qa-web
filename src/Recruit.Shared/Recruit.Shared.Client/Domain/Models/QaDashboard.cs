using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

namespace Recruit.Vacancies.Client.Domain.Models;
public record QaDashboard
{
    public int TotalVacanciesForReview { get; init; }
    public int TotalVacanciesBrokenSla { get; init; }
    public int TotalVacanciesResubmitted { get; init; }
    public int TotalVacanciesSubmittedTwelveTwentyFourHours { get; init; }

    public static QaDashboard FromApiResponse(GetQaDashboardApiResponse response)
    {
        if (response == null)
            return new QaDashboard();

        return new QaDashboard
        {
            TotalVacanciesForReview = response.TotalVacanciesForReview,
            TotalVacanciesBrokenSla = response.TotalVacanciesBrokenSla,
            TotalVacanciesResubmitted = response.TotalVacanciesResubmitted,
            TotalVacanciesSubmittedTwelveTwentyFourHours = response.TotalVacanciesSubmittedTwelveTwentyFourHours
        };
    }
}
