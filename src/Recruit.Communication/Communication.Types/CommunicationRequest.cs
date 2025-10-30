using System;
using System.Collections.Generic;

namespace Recruit.Communication.Types;

public class CommunicationRequest()
{
    public Guid RequestId { get; set ;}
    public string RequestType { get; set; }
    public DateTime RequestDateTime { get; set; } = DateTime.UtcNow;
    public string ParticipantsResolverName { get; set; }
    public string TemplateProviderName { get; set; }
    public List<Entity> Entities { get; set; } = new List<Entity>();
    public List<CommunicationDataItem> DataItems { get; set; } = new List<CommunicationDataItem>();

    public CommunicationRequest(string requestType, string participantsResolverName, string templateProviderName) : this()
    {
        RequestId = Guid.NewGuid();
        RequestType = requestType;
        ParticipantsResolverName = participantsResolverName;
        TemplateProviderName = templateProviderName;
    }

    public void AddEntity(string entityType, object entityId)
    {
        Entities.Add(new Entity(entityType, entityId));
    }
}