using System;

namespace Recruit.Vacancies.Client.Domain.Entities;

[Flags]
public enum NotificationTypes
{
    None = 0,
    VacancyRejected = 1,
    VacancyClosingSoon = 1 << 1,
    ApplicationSubmitted = 1 << 2,
    VacancySentForReview = 1 << 3,
    VacancyRejectedByEmployer = 1 << 4
}