using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class ProviderBlockedOnVacancyEvent : EventBase, INotification
{
    public Guid VacancyId { get; set; }
    public VacancyUser QaVacancyUser { get; set; }
    public long Ukprn { get; set; }
    public DateTime BlockedDate { get; set; }
    public long VacancyReference { get; set; }
}