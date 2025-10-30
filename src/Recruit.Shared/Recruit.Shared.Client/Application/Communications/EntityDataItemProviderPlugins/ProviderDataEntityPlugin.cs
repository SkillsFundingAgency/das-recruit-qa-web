using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Exceptions;
using Recruit.Communication.Types.Interfaces;
using static Recruit.Vacancies.Client.Application.Communications.CommunicationConstants;

namespace Recruit.Vacancies.Client.Application.Communications.EntityDataItemProviderPlugins;

public class ProviderDataEntityPlugin(ITrainingProviderSummaryProvider trainingProviderSummaryProvider)
    : IEntityDataItemProvider
{
    public string EntityType => CommunicationConstants.EntityTypes.Provider;

    public async Task<IEnumerable<CommunicationDataItem>> GetDataItemsAsync(object entityId)
    {
        if (long.TryParse(entityId.ToString(), out var ukprn) == false)
        {
            throw new InvalidEntityIdException(EntityType, nameof(ApprenticeshipServiceUrlDataEntityPlugin));
        }
            
        var summary = await trainingProviderSummaryProvider.GetAsync(ukprn);

        return new [] { new CommunicationDataItem(DataItemKeys.Provider.ProviderName, summary.ProviderName ) };
    }
}