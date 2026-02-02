using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.TrainingProviderSummaryProvider;

/// <summary>
/// Returns providers from RoATP (Register of Apprenticeship Training Providers)
/// </summary>
public class TrainingProviderSummaryProvider(ITrainingProviderService trainingProviderService)
    : ITrainingProviderSummaryProvider
{
    public async Task<TrainingProviderSummary> GetAsync(long ukprn)
    {
        if (ukprn == EsfaTestTrainingProvider.Ukprn)
            return new TrainingProviderSummary { Ukprn = EsfaTestTrainingProvider.Ukprn, ProviderName = EsfaTestTrainingProvider.Name };

        var provider = await trainingProviderService.GetProviderAsync(ukprn);
            
        return new TrainingProviderSummary
        {
            Ukprn = provider.Ukprn.Value,
            ProviderName = provider.Name
        };
    }
}