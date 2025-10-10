using System;
using System.Collections.Generic;

namespace Recruit.Communication.Types;

public class AggregateCommunicationComposeRequest(
    string userId,
    IEnumerable<Guid> messageIds,
    AggregateCommunicationRequest aggregateCommunicationRequest)
{
    public string UserId { get; } = userId;
    public IEnumerable<Guid> MessageIds { get; } = messageIds;
    public AggregateCommunicationRequest AggregateCommunicationRequest { get; } = aggregateCommunicationRequest;
}