using System;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

// Note: This is an externally published event.
public class CandidateDeleteEvent : EventBase, INotification
{
    public Guid CandidateId { get; set; }
}