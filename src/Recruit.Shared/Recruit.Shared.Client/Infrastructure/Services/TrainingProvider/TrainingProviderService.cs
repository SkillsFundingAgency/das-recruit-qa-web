using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Domain.Models;
using Recruit.Vacancies.Client.Infrastructure.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;

public class TrainingProviderService(
    ILogger<TrainingProviderService> logger,
    IRecruitQaOuterApiVacancyClient recruitQaOuterApiVacancyClient)
    : ITrainingProviderService
{
    public async Task<Domain.Entities.TrainingProvider> GetProviderAsync(long ukprn)
    {
        if (ukprn == EsfaTestTrainingProvider.Ukprn)
            return GetEsfaTestTrainingProvider();
            
        try
        {
            var provider = await GetProviderDetails(ukprn);
            return TrainingProviderMapper.MapFromApiProvider(provider);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to retrieve provider information for UKPRN: {Ukprn}", ukprn);
            return null;
        }
    }
    

    public async Task<Provider> GetProviderDetails(long ukprn)
    {
        logger.LogTrace("Getting Provider Details from Outer Api");

        var retryPolicy = PollyRetryPolicy.GetPolicy();

        var result = await retryPolicy.Execute(
            _ => recruitQaOuterApiVacancyClient.GetProviderAsync(ukprn),
            new Dictionary<string, object> {{"apiCall", "Providers"}});

        return result;
    }

    private static Domain.Entities.TrainingProvider GetEsfaTestTrainingProvider()
    {
        return new Domain.Entities.TrainingProvider
        {
            Ukprn = EsfaTestTrainingProvider.Ukprn,
            Name = EsfaTestTrainingProvider.Name,
            Address = new Domain.Entities.Address
            {
                AddressLine1 = EsfaTestTrainingProvider.AddressLine1,
                AddressLine2 = EsfaTestTrainingProvider.AddressLine2,
                AddressLine3 = EsfaTestTrainingProvider.AddressLine3,
                AddressLine4 = EsfaTestTrainingProvider.AddressLine4,
                Postcode = EsfaTestTrainingProvider.Postcode
            }
        };
    }
}