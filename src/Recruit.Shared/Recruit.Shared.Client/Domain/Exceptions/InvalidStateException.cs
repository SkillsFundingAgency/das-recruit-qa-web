using System;

namespace Recruit.Vacancies.Client.Domain.Exceptions;

[Serializable]
public class InvalidStateException(string message) : RecruitException(message);