using System;

namespace Recruit.Communication.Types;

public class CommunicationMessageIdentifier
{
    public Guid Id { get; }

    public CommunicationMessageIdentifier(Guid id)
    {
        Id = id;
    }
}