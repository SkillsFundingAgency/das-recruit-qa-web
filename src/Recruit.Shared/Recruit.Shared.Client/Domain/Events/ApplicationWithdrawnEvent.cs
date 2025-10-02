using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

// Note: Doesn't implement IApplicationReviewEvent as it's an externally published event.
public class ApplicationWithdrawnEvent : EventBase, INotification, IVacancyEvent
{
    public Guid CandidateId { get; set; }
    public long VacancyReference { get; set; }
    public Guid VacancyId { get; }
}