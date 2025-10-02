using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Communication.Types.Interfaces;

public interface ICompositeDataItemProvider
{
    string ProviderName { get; }
    Task<IEnumerable<CommunicationDataItem>> GetConsolidatedMessageDataItemsAsync(CommunicationMessage aggregateMessage, IEnumerable<CommunicationMessage> messages);
}