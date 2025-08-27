using System.Collections.Generic;
using System.Threading.Tasks;
using Recruit.Communication.Types;

namespace Communication.Core;

public interface IAggregateCommunicationProcessor
{
    Task<CommunicationMessage> CreateAggregateMessageAsync(AggregateCommunicationRequest request, IEnumerable<CommunicationMessage> messages);
}