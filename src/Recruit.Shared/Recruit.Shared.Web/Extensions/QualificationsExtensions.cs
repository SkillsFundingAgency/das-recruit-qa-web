using System.Collections.Generic;
using System.Linq;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Shared.Web.Extensions;

public static class QualificationsExtensions
{
    public static IEnumerable<string> AsText(this IEnumerable<Qualification> qualifications)
    {
        if (qualifications == null)
        {
            return Enumerable.Empty<string>();
        }
            
        return qualifications.Select(q =>
        {
            var additionalText = !string.IsNullOrEmpty(q.OtherQualificationName) ? $" ({q.OtherQualificationName})" :"";
            var levelText = q.Level.HasValue ? $" (Level {q.Level})":"";
            string qualificationType = $"{q.QualificationType}{levelText}{additionalText} {q.Subject} (Grade {q.Grade}) {q.Weighting.GetDisplayName().ToLower()}";
            return qualificationType;
        });
    }
}