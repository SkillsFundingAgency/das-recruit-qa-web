using System.Threading.Tasks;
using Recruit.Communication.Types;

namespace Communication.Core;

public interface IAggregateCommunicationComposeQueuePublisher
{
    Task AddMessageAsync(AggregateCommunicationComposeRequest message);
}