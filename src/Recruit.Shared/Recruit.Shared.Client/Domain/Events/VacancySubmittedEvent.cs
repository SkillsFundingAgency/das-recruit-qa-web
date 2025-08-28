using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancySubmittedEvent : EventBase, INotification, IVacancyEvent
{
    public string EmployerAccountId { get; set; }
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
}