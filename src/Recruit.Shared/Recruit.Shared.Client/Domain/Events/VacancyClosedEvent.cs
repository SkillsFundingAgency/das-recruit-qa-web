using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancyClosedEvent : EventBase, INotification, IVacancyEvent, NServiceBus.IEvent
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
}