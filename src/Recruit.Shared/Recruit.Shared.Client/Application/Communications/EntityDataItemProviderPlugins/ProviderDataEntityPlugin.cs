using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Application.Providers;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Exceptions;
using Recruit.Communication.Types.Interfaces;
using static Esfa.Recruit.Vacancies.Client.Application.Communications.CommunicationConstants;

namespace Esfa.Recruit.Vacancies.Client.Application.Communications.EntityDataItemProviderPlugins;

public class ProviderDataEntityPlugin : IEntityDataItemProvider
{
    public string EntityType => CommunicationConstants.EntityTypes.Provider;
    public ITrainingProviderSummaryProvider _trainingProviderSummaryProvider;

    public ProviderDataEntityPlugin(ITrainingProviderSummaryProvider trainingProviderSummaryProvider)
    {
        _trainingProviderSummaryProvider = trainingProviderSummaryProvider;
    }

    public async Task<IEnumerable<CommunicationDataItem>> GetDataItemsAsync(object entityId)
    {
        if (long.TryParse(entityId.ToString(), out var ukprn) == false)
        {
            throw new InvalidEntityIdException(EntityType, nameof(ApprenticeshipServiceUrlDataEntityPlugin));
        }
            
        var summary = await _trainingProviderSummaryProvider.GetAsync(ukprn);

        return new [] { new CommunicationDataItem(DataItemKeys.Provider.ProviderName, summary.ProviderName ) };
    }
}