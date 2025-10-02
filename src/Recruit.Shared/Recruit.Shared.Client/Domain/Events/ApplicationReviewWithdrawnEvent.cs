using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Domain.Events;

public class ApplicationReviewWithdrawnEvent : EventBase, INotification, IApplicationReviewEvent
{
    public long VacancyReference { get; set; }
}