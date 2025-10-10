using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Extensions;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Vacancy;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Projections;

public class PublishedVacancyProjectionService(
    ILogger<PublishedVacancyProjectionService> logger,
    IQueryStoreWriter queryStoreWriter,
    IVacancyQuery vacancyQuery,
    IApprenticeshipProgrammeProvider apprenticeshipProgrammeProvider,
    ITimeProvider timeProvider)
    : IPublishedVacancyProjectionService
{
    public async Task ReGeneratePublishedVacanciesAsync()
    {            
        var liveVacancyIdsTask = vacancyQuery.GetVacanciesByStatusAsync<VacancyIdentifier>(VacancyStatus.Live);
        var closedVacancyIdsTask = vacancyQuery.GetVacanciesByStatusAsync<VacancyIdentifier>(VacancyStatus.Closed);
        var programmesTask = apprenticeshipProgrammeProvider.GetApprenticeshipProgrammesAsync();

        await Task.WhenAll(liveVacancyIdsTask, programmesTask, closedVacancyIdsTask);

        var liveVacancyIds = liveVacancyIdsTask.Result.Select(v => v.Id).ToList();
        var closedVacancyIds = closedVacancyIdsTask.Result.Select(v => v.Id).ToList();
        var programmes = programmesTask.Result.Select(c=>(ApprenticeshipProgramme)c).ToList();
            
        var regenerateLiveVacanciesViewsTask = RegenerateLiveVacancies(liveVacancyIds, programmes);
        var regenerateClosedVacanciesViewsTask = RegenerateClosedVacancies(closedVacancyIds, programmes);

        await Task.WhenAll(regenerateClosedVacanciesViewsTask, regenerateLiveVacanciesViewsTask);
    }

    private async Task RegenerateLiveVacancies(List<Guid> liveVacancyIds, List<ApprenticeshipProgramme> programmes)
    {
        var watch = Stopwatch.StartNew();

        var deletedCount = await queryStoreWriter.DeleteAllLiveVacancies();

        foreach (var vacancyId in liveVacancyIds)
        {
            var vacancy = await vacancyQuery.GetVacancyAsync(vacancyId);

            var programme = programmes.Single(p => p.Id == vacancy.ProgrammeId);
            var vacancyProjection = vacancy.ToVacancyProjectionBase<LiveVacancy>(programme,
                () => QueryViewType.LiveVacancy.GetIdValue(vacancy.VacancyReference.ToString()), timeProvider);

            await queryStoreWriter.UpdateLiveVacancyAsync(vacancyProjection);
        }

        watch.Stop();

        logger.LogInformation("Recreated LiveVacancy projections. Deleted:{deletedCount}. Inserted:{insertedCount}. executionTime:{executionTimeMs}ms", deletedCount, liveVacancyIds.Count, watch.ElapsedMilliseconds);
    }

    private async Task RegenerateClosedVacancies(List<Guid> closedVacancyIds, List<ApprenticeshipProgramme> programmes)
    {
        var watch = Stopwatch.StartNew();

        var deletedCount = await queryStoreWriter.DeleteAllClosedVacancies();

        foreach (var vacancyId in closedVacancyIds)
        {
            var vacancy = await vacancyQuery.GetVacancyAsync(vacancyId);

            var programme = programmes.Single(p => p.Id == vacancy.ProgrammeId);
            var vacancyProjection = vacancy.ToVacancyProjectionBase<ClosedVacancy>(programme,
                () => QueryViewType.ClosedVacancy.GetIdValue(vacancy.VacancyReference.ToString()), timeProvider);

            await queryStoreWriter.UpdateClosedVacancyAsync(vacancyProjection);
        }

        watch.Stop();

        logger.LogInformation("Recreated ClosedVacancy projections. Deleted:{deletedCount}. Inserted:{insertedCount}. executionTime:{executionTimeMs}ms", deletedCount, closedVacancyIds.Count, watch.ElapsedMilliseconds);
    }
}