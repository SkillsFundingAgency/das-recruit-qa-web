using System;
using Recruit.Vacancies.Client.Domain.Exceptions;

namespace Recruit.Qa.Web.Exceptions;

[Serializable]
public class NotFoundException(string message) : RecruitException(message);