using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancyDeletedEvent : EventBase, INotification, IVacancyEvent
{
    public Guid VacancyId { get; set; }
}