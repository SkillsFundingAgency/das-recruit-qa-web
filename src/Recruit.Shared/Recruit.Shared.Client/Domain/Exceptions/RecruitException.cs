using System;

namespace Recruit.Vacancies.Client.Domain.Exceptions;

public abstract class RecruitException(string message) : Exception(message);