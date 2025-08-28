using System;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Extensions;

namespace Recruit.Shared.Web.Extensions;

[Obsolete("Use ToDisplayText() extension method instead")]
public static class WageExtensions
{
    public static string ToText(this Wage wage, DateTime? expectedStartDate)
    {
        return wage.ToDisplayText(expectedStartDate);
    }
}