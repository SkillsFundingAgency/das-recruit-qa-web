using System;

namespace Recruit.Communication.Types.Exceptions;

public class UnknownRecipientResolverTypeException(string message) : Exception(message);