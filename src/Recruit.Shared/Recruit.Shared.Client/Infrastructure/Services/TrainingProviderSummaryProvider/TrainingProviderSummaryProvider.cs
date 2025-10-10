﻿using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Models;
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

    /// <summary>
    /// Method to check if the given ukprn number is a valid training provider with Main or Employer Profile with Status not equal to "Not Currently Starting New Apprentices".
    /// </summary>
    /// <param name="ukprn">ukprn number.</param>
    /// <returns>boolean.</returns>
    public async Task<bool> IsTrainingProviderMainOrEmployerProfile(long ukprn)
    {
        if (ukprn == EsfaTestTrainingProvider.Ukprn)
            return true;
            
        var provider = await trainingProviderService.GetProviderDetails(ukprn);

        // logic to filter only Training provider with Main & Employer Profiles and Status Id not equal to "Not Currently Starting New Apprentices"
        return provider != null &&
               (provider.ProviderTypeId.Equals((short) ProviderTypeIdentifier.MainProvider) ||
                provider.ProviderTypeId.Equals((short) ProviderTypeIdentifier.EmployerProvider)) &&
               !provider.StatusId.Equals((short) ProviderStatusType.ActiveButNotTakingOnApprentices);
    }
}