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

public class UpdateLiveVacancyOnVacancyChange : INotificationHandler<VacancyApprovedEvent>, INotificationHandler<VacancyPublishedEvent>
{
    private readonly IVacancyRepository _repository;
    private readonly ILogger<UpdateLiveVacancyOnVacancyChange> _logger;
    private readonly IMessaging _messaging;
    private readonly IApprenticeshipProgrammeProvider _apprenticeshipProgrammeProvider;
    private readonly IQueryStoreWriter _queryStoreWriter;
    private readonly ITimeProvider _timeProvider;

    public UpdateLiveVacancyOnVacancyChange(IQueryStoreWriter queryStoreWriter, ILogger<UpdateLiveVacancyOnVacancyChange> logger, 
        IVacancyRepository repository, IMessaging messaging, IApprenticeshipProgrammeProvider apprenticeshipProgrammeProvider, ITimeProvider timeProvider)
    {
        _queryStoreWriter = queryStoreWriter;
        _logger = logger;
        _repository = repository;
        _messaging = messaging;
        _apprenticeshipProgrammeProvider = apprenticeshipProgrammeProvider;
        _timeProvider = timeProvider;
    }
        
    public Task Handle(VacancyApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {notificationType} for vacancyId: {vacancyId}", notification?.GetType().Name, notification?.VacancyId);
            
        //For now approved vacancies are immediately made Live
        return _messaging.SendCommandAsync(new PublishVacancyCommand
        {
            VacancyId = notification.VacancyId
        });
    }

    public async Task Handle(VacancyPublishedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling VacancyPublishedEvent vacancy {vacancyId}.", notification.VacancyId);

        var vacancy = await _repository.GetVacancyAsync(notification.VacancyId);
            
        var programme = await _apprenticeshipProgrammeProvider.GetApprenticeshipProgrammeAsync(vacancy.ProgrammeId);

        var liveVacancy = vacancy.ToVacancyProjectionBase<LiveVacancy>((ApprenticeshipProgramme)programme, () => QueryViewType.LiveVacancy.GetIdValue(vacancy.VacancyReference.ToString()), _timeProvider);
        _logger.LogInformation("Updating LiveVacancy in query store for vacancy {vacancyId} reference {vacancyReference}.", liveVacancy.VacancyId, liveVacancy.VacancyReference);

        try
        {
            await _queryStoreWriter.UpdateLiveVacancyAsync(liveVacancy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling VacancyPublishedEvent vacancy {vacancyId}.", notification.VacancyId);
            throw;
        }
    }
}