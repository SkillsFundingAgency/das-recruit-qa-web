using System;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class LiveVacancyClosingDateChangedEvent : EventBase, INotification
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
    public DateTime NewClosingDate { get; set; }
}