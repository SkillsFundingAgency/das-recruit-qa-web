using System;

namespace Recruit.Communication.Types;

public class AggregateCommunicationRequest(
    Guid requestId,
    string requestType,
    DeliveryFrequency frequency,
    DateTime requestDateTime,
    DateTime fromDateTime,
    DateTime toDateTime)
{
    public Guid RequestId { get; } = requestId;
    public DateTime RequestDateTime { get; } = requestDateTime;
    public DeliveryFrequency Frequency { get; } = frequency;
    public string RequestType { get; } = requestType;
    public DateTime FromDateTime { get; } = fromDateTime;
    public DateTime ToDateTime { get; } = toDateTime;

    // end time
}