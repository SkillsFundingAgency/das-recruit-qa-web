using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.Services.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class UpdateVacancyApplicationsOnApplicationReviewChange(
    ILogger<UpdateVacancyApplicationsOnApplicationReviewChange> logger,
    IVacancyApplicationsProjectionService projectionService)
    :
        INotificationHandler<ApplicationReviewCreatedEvent>,
        INotificationHandler<ApplicationReviewWithdrawnEvent>,
        INotificationHandler<ApplicationReviewDeletedEvent>,
        INotificationHandler<ApplicationReviewedEvent>
{
    public Task Handle(ApplicationReviewCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(ApplicationReviewWithdrawnEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(ApplicationReviewedEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(ApplicationReviewDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    private Task Handle(IApplicationReviewEvent notification)
    {
        logger.LogInformation("Handling {notificationType} for vacancyReference: {vacancyReference}", notification.GetType().Name, notification.VacancyReference);

        return projectionService.UpdateVacancyApplicationsAsync(notification.VacancyReference);
    }
}