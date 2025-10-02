using System;
using Recruit.Vacancies.Client.Domain.Exceptions;

namespace Recruit.Vacancies.Client.Infrastructure.Exceptions;

[Serializable]
public class VacancyNotFoundException : RecruitException
{
    public VacancyNotFoundException(string message) : base(message) { }
}