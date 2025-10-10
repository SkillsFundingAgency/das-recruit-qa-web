﻿using Recruit.Vacancies.Client.Application.Events;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.EventHandlers;

public class RePublishEventToEventStoreEventHandler(
    ILogger<RePublishEventToEventStoreEventHandler> logger,
    IEventStore eventStore)
    :
        INotificationHandler<DraftVacancyUpdatedEvent>,
        INotificationHandler<VacancySubmittedEvent>,
        INotificationHandler<VacancyReviewedEvent>,
        INotificationHandler<VacancyReferredEvent>,
        INotificationHandler<VacancyRejectedEvent>,
        INotificationHandler<VacancyReviewApprovedEvent>,
        INotificationHandler<VacancyReviewReferredEvent>,
        INotificationHandler<SetupEmployerEvent>,
        INotificationHandler<SetupProviderEvent>,
        INotificationHandler<VacancyReviewCreatedEvent>,
        INotificationHandler<ProviderBlockedEvent>,
        INotificationHandler<ProviderBlockedOnLegalEntityEvent>,
        INotificationHandler<ProviderBlockedOnVacancyEvent>,
        INotificationHandler<VacancyClosedEvent>,
        INotificationHandler<LiveVacancyUpdatedEvent>,
        INotificationHandler<ApplicationSubmittedEvent>,
        INotificationHandler<ApplicationWithdrawnEvent>
{
    public Task Handle(VacancySubmittedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyReviewedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyReferredEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyRejectedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification); 

    public Task Handle(DraftVacancyUpdatedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyReviewApprovedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyReviewReferredEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(SetupEmployerEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(SetupProviderEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyReviewCreatedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(ProviderBlockedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(ProviderBlockedOnLegalEntityEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(ProviderBlockedOnVacancyEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(VacancyClosedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);

    public Task Handle(LiveVacancyUpdatedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);
    public Task Handle(ApplicationSubmittedEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);
    public Task Handle(ApplicationWithdrawnEvent notification, CancellationToken cancellationToken)
        => HandleUsingEventStore(notification);
    private async Task HandleUsingEventStore(IEvent @event)
    {
        logger.LogInformation("Re-publishing event {eventType} to event store", @event.GetType().Name);

        await eventStore.Add(@event);
    }
}