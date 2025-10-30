using System;
using Recruit.Vacancies.Client.Domain.Exceptions;

namespace Recruit.Vacancies.Client.Infrastructure.Exceptions;

[Serializable]
public class ReportNotFoundException(string message) : RecruitException(message);