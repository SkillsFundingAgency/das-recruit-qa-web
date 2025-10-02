using System;

namespace Recruit.Vacancies.Client.Domain.Exceptions;

[Serializable]
public class AuthorisationException : RecruitException
{
    public AuthorisationException(string message) : base(message) { }
}