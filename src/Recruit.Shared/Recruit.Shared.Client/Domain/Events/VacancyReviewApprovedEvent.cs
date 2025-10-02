using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancyReviewApprovedEvent : EventBase, INotification, IVacancyReviewEvent
{
    public long VacancyReference { get; set; }

    public Guid ReviewId { get; set; }
}