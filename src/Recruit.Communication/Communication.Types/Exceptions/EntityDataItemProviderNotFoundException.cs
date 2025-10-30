using System;

namespace Recruit.Communication.Types.Exceptions;

public class EntityDataItemProviderNotFoundException(string entityType)
    : Exception($"Unable to resolve entity data item provider for entity type '{entityType}'");