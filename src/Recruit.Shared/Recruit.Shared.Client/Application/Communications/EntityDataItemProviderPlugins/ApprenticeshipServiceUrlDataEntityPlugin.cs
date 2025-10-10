using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.Options;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Exceptions;
using Recruit.Communication.Types.Interfaces;

namespace Recruit.Vacancies.Client.Application.Communications.EntityDataItemProviderPlugins;

public class ApprenticeshipServiceUrlDataEntityPlugin(
    IVacancyRepository vacancyRepository,
    IOptions<CommunicationsConfiguration> communicationsConfiguration)
    : IEntityDataItemProvider
{
    private readonly CommunicationsConfiguration _communicationsConfiguration = communicationsConfiguration.Value;
    public string EntityType => CommunicationConstants.EntityTypes.ApprenticeshipServiceUrl;

    public async Task<IEnumerable<CommunicationDataItem>> GetDataItemsAsync(object entityId)
    {
        if (long.TryParse(entityId.ToString(), out var vacancyReference) == false)
        {
            throw new InvalidEntityIdException(EntityType, nameof(ApprenticeshipServiceUrlDataEntityPlugin));
        }

        var vacancy = await vacancyRepository.GetVacancyAsync(vacancyReference);

            
        return new [] { GetApplicationUrlDataItem(vacancy) };
    }

    private CommunicationDataItem GetApplicationUrlDataItem(Vacancy vacancy)
    {
        var url = string.Empty;
        if (vacancy.OwnerType == OwnerType.Employer || vacancy.Status == VacancyStatus.Review)
        {
            var baseUri = new Uri(_communicationsConfiguration.EmployersApprenticeshipServiceUrl);
            var uri = new Uri(baseUri, vacancy.EmployerAccountId);
            url = uri.ToString();
        }
        else
        {
            var baseUri = new Uri(_communicationsConfiguration.ProvidersApprenticeshipServiceUrl);
            var uri = new Uri(baseUri, $"{vacancy.TrainingProvider.Ukprn}/vacancies");
            url = uri.ToString();
        }

        return new CommunicationDataItem(CommunicationConstants.DataItemKeys.ApprenticeshipService.ApprenticeshipServiceUrl, url);
    }
}