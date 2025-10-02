using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

// Note: Doesn't implement IApplicationReviewEvent as it's an externally published event.
public class ApplicationSubmittedEvent : EventBase, INotification, IVacancyEvent
{
    public Entities.Application Application { get; set; }
    public Guid VacancyId { get; set; }
}