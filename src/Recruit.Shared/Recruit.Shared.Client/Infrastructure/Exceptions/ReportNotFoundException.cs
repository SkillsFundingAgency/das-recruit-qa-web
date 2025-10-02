using System;
using Recruit.Vacancies.Client.Domain.Exceptions;

namespace Recruit.Vacancies.Client.Infrastructure.Exceptions;

[Serializable]
public class ReportNotFoundException : RecruitException
{
    public ReportNotFoundException(string message) : base(message) { }
}