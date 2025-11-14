using System;

namespace Recruit.Vacancies.Client.Domain.Exceptions;

[Serializable]
public class AuthorisationException(string message) : RecruitException(message);