using System;

namespace Recruit.Communication.Types.Exceptions;

public class InvalidEntityIdException(string entityType, string entityDataItemProviderName)
    : Exception($"Unexpected entity id received by '{entityDataItemProviderName}' for type '{entityType}'");