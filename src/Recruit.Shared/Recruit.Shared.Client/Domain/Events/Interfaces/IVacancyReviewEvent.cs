using System;

namespace Recruit.Vacancies.Client.Domain.Events.Interfaces;

public interface IVacancyReviewEvent
{
    long VacancyReference { get; }
    Guid ReviewId { get; }
}