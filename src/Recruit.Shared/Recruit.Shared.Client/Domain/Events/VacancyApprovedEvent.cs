using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancyApprovedEvent : EventBase, INotification, IVacancyEvent, NServiceBus.IEvent
{
    public string AccountLegalEntityPublicHashedId { get; set; }
    public long Ukprn { get; set; }
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
}