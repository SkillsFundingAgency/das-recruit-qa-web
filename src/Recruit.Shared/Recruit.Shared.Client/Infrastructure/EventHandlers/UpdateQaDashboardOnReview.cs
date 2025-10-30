using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Events.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.Services.Projections;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class UpdateQaDashboardOnReview(
    ILogger<UpdateQaDashboardOnReview> logger,
    IQaDashboardProjectionService qaDashboardService)
    : INotificationHandler<VacancyReviewApprovedEvent>,
        INotificationHandler<VacancyReviewReferredEvent>,
        INotificationHandler<VacancyReviewWithdrawnEvent>
{
    public Task Handle(VacancyReviewCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(VacancyReviewApprovedEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(VacancyReviewReferredEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    public Task Handle(VacancyReviewWithdrawnEvent notification, CancellationToken cancellationToken)
    {
        return Handle(notification);
    }

    private Task Handle(IVacancyReviewEvent notification)
    {
        logger.LogInformation("Handling {notificationType}", notification.GetType().Name);

        return qaDashboardService.RebuildQaDashboardAsync();
    }
}