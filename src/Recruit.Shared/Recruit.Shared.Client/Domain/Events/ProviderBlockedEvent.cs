using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class ProviderBlockedEvent : EventBase, INotification
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public DateTime BlockedDate { get; set; }
    public VacancyUser QaVacancyUser { get; set; }
}