using System;
using System.Linq.Expressions;
using Recruit.Vacancies.Client.Application.Rules.Extensions;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services;

public static class FieldIdResolver
{
    public static string ToFieldId<P>(Expression<Func<Vacancy, P>> fieldName)
    {
        return fieldName.GetQualifiedFieldId();
    }
}