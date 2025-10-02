using System;

namespace Recruit.Communication.Types.Exceptions;

public class UnknownRecipientResolverTypeException : Exception
{
    public UnknownRecipientResolverTypeException(string message) : base(message) { }
}