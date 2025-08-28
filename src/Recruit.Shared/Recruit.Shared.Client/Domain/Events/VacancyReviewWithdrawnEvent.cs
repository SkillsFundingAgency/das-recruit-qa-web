using System;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class VacancyReviewWithdrawnEvent : EventBase, INotification, IVacancyEvent, IVacancyReviewEvent
{
    public Guid VacancyId { get; }

    public long VacancyReference { get;}

    public Guid ReviewId { get; }

    public VacancyReviewWithdrawnEvent(Guid vacancyId, long vacancyReference, Guid reviewId)
    {
        VacancyId = vacancyId;
        VacancyReference = vacancyReference;
        ReviewId = reviewId;
    }
}