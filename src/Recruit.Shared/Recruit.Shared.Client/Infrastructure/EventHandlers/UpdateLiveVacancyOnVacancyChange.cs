using System;
using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Extensions;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class UpdateLiveVacancyOnVacancyChange(
    IQueryStoreWriter queryStoreWriter,
    ILogger<UpdateLiveVacancyOnVacancyChange> logger,
    IVacancyRepository repository,
    IMessaging messaging,
    IApprenticeshipProgrammeProvider apprenticeshipProgrammeProvider,
    ITimeProvider timeProvider)
    : INotificationHandler<VacancyApprovedEvent>, INotificationHandler<VacancyPublishedEvent>
{
    public Task Handle(VacancyApprovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {notificationType} for vacancyId: {vacancyId}", notification?.GetType().Name, notification?.VacancyId);
            
        //For now approved vacancies are immediately made Live
        return messaging.SendCommandAsync(new PublishVacancyCommand
        {
            VacancyId = notification.VacancyId
        });
    }

    public async Task Handle(VacancyPublishedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling VacancyPublishedEvent vacancy {vacancyId}.", notification.VacancyId);

        var vacancy = await repository.GetVacancyAsync(notification.VacancyId);
            
        var programme = await apprenticeshipProgrammeProvider.GetApprenticeshipProgrammeAsync(vacancy.ProgrammeId);

        var liveVacancy = vacancy.ToVacancyProjectionBase<LiveVacancy>((ApprenticeshipProgramme)programme, () => QueryViewType.LiveVacancy.GetIdValue(vacancy.VacancyReference.ToString()), timeProvider);
        logger.LogInformation("Updating LiveVacancy in query store for vacancy {vacancyId} reference {vacancyReference}.", liveVacancy.VacancyId, liveVacancy.VacancyReference);

        try
        {
            await queryStoreWriter.UpdateLiveVacancyAsync(liveVacancy);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling VacancyPublishedEvent vacancy {vacancyId}.", notification.VacancyId);
            throw;
        }
    }
}