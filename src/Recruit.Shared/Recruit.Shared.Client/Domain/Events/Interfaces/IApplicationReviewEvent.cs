namespace Recruit.Vacancies.Client.Domain.Events.Interfaces;

public interface IApplicationReviewEvent
{
    long VacancyReference { get; }
}