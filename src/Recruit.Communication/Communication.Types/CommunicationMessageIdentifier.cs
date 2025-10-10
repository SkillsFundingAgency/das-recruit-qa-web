using System;

namespace Recruit.Communication.Types;

public class CommunicationMessageIdentifier(Guid id)
{
    public Guid Id { get; } = id;
}