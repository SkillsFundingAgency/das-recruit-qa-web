using System;

namespace Recruit.Communication.Types.Exceptions;

public class TemplateIdProviderNotFoundException(string originatingServiceName)
    : Exception($"Unable to resolve template id provider for service {originatingServiceName}");