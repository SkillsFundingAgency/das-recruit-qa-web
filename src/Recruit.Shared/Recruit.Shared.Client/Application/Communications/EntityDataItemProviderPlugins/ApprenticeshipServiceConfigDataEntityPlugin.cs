using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Recruit.Communication.Types;
using Recruit.Communication.Types.Interfaces;

namespace Recruit.Vacancies.Client.Application.Communications.EntityDataItemProviderPlugins;

public class ApprenticeshipServiceConfigDataEntityPlugin(
    IOptions<CommunicationsConfiguration> communicationsConfiguration)
    : IEntityDataItemProvider
{
    private readonly CommunicationsConfiguration _communicationsConfiguration = communicationsConfiguration.Value;
    public string EntityType => CommunicationConstants.EntityTypes.ApprenticeshipServiceConfig;

    public Task<IEnumerable<CommunicationDataItem>> GetDataItemsAsync(object entityId)
    {
        IEnumerable<CommunicationDataItem> dataItems = new [] { GetHelpdeskNumberDataItem() };
        return Task.FromResult(dataItems);
    }

    private CommunicationDataItem GetHelpdeskNumberDataItem()
    {
        return new CommunicationDataItem(CommunicationConstants.DataItemKeys.ApprenticeshipService.HelpdeskPhoneNumber, CommunicationConstants.HelpdeskPhoneNumber);
    }
}