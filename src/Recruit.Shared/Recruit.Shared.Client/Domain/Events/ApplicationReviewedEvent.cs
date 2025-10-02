using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class ApplicationReviewedEvent : EventBase, INotification, IApplicationReviewEvent
{
    public long VacancyReference { get; set; }
    public Guid CandidateId { get; set; }
    public ApplicationReviewStatus Status { get; set; }
    public string CandidateFeedback { get; set; }
}