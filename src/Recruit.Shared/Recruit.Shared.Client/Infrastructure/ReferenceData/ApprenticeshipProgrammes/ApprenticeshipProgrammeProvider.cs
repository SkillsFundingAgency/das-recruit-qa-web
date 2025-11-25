using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.FeatureToggle;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;

public class ApprenticeshipProgrammeProvider(
    ICache cache,
    ITimeProvider timeProvider,
    IRecruitOuterApiClient outerApiClient,
    IFeature feature)
    : IApprenticeshipProgrammeProvider
{
    private const int DummyStandardProgrammeId = 999999;

    public async Task<IApprenticeshipProgramme> GetApprenticeshipProgrammeAsync(string programmeId)
    {
        var apprenticeships = await GetApprenticeshipProgrammesAsync(true);

        return apprenticeships?.SingleOrDefault(x => x.Id == programmeId);
    }

    public async Task<IEnumerable<IApprenticeshipProgramme>> GetApprenticeshipProgrammesAsync(bool includeExpired = false)
    {
        var queryItem = await GetApprenticeshipProgrammes();
        return includeExpired ?
            queryItem.Data :
            queryItem.Data.Where(x => x.IsActive || (x.ApprenticeshipType == TrainingType.Foundation && IsStandardActive(x.EffectiveTo, x.LastDateStarts)));
    }

    private Task<ApprenticeshipProgrammes> GetApprenticeshipProgrammes()
    {
        var includeFoundationApprenticeships = feature.IsFeatureEnabled(FeatureNames.FoundationApprenticeships);

        return cache.CacheAsideAsync(CacheKeys.ApprenticeshipProgrammes,
            timeProvider.NextDay6am,
            async () =>
            {
                var result = await outerApiClient.Get<GetTrainingProgrammesResponse>(new GetTrainingProgrammesRequest(includeFoundationApprenticeships));
                var trainingProgrammes = result.TrainingProgrammes.Select(c => (ApprenticeshipProgramme)c).ToList();
                trainingProgrammes.Add(GetDummyProgramme());
                return new ApprenticeshipProgrammes
                {
                    Data = trainingProgrammes
                };
            });
    }
        
    private static bool IsStandardActive(DateTime? effectiveTo, DateTime? lastDateStarts)
    {
        return (!effectiveTo.HasValue || effectiveTo.Value.Date >= DateTime.UtcNow.Date)
               && (!lastDateStarts.HasValue || lastDateStarts.Value.Date >= DateTime.UtcNow.Date);
    }

    private static ApprenticeshipProgramme GetDummyProgramme() =>
        new()
        {
            Id = DummyStandardProgrammeId.ToString(),
            Title = "To be confirmed",
            IsActive = true,
            ApprenticeshipType = TrainingType.Standard,
            ApprenticeshipLevel = ApprenticeshipLevel.Unknown,
            EffectiveTo = DateTime.UtcNow.AddYears(1),
            LastDateStarts = DateTime.UtcNow.AddYears(1)
        };
}