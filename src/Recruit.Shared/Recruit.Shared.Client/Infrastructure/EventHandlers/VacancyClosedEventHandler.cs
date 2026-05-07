using System;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Extensions;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class VacancyClosedEventHandler(
    ILogger<VacancyClosedEventHandler> logger,
    IQueryStoreWriter queryStore,
    IVacancyRepository repository,
    IApprenticeshipProgrammeProvider apprenticeshipProgrammeProvider,
    ITimeProvider timeProvider,
    IQueryStoreReader queryStoreReader)
    : INotificationHandler<VacancyClosedEvent>
{
    public async Task Handle(VacancyClosedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting LiveVacancy {vacancyReference} from query store.",
            notification.VacancyReference);
            
        await queryStore.DeleteLiveVacancyAsync(notification.VacancyReference);
        await CreateClosedVacancyProjection(notification.VacancyId);
    }

    private async Task CreateClosedVacancyProjection(Guid vacancyId)
    {
        var vacancy = await repository.GetVacancyAsync(vacancyId);
            
        var queryResult = await queryStoreReader.GetClosedVacancy(vacancy.VacancyReference.Value);

        if (queryResult != null)
        {
            logger.LogInformation($"Vacancy {vacancy.VacancyReference} already closed. Skipping notification.");
            return;
        }
            
        var programme = await apprenticeshipProgrammeProvider.GetApprenticeshipProgrammeAsync(vacancy.ProgrammeId);

        await queryStore.UpdateClosedVacancyAsync(vacancy.ToVacancyProjectionBase<ClosedVacancy>((ApprenticeshipProgramme)programme, () => QueryViewType.ClosedVacancy.GetIdValue(vacancy.VacancyReference.ToString()), timeProvider));
    }
}