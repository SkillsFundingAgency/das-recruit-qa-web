using System;

namespace Recruit.Vacancies.Client.Domain.Exceptions;

[Serializable]
public class InvalidStateException : RecruitException
{
    public InvalidStateException(string message) : base(message) { }
}