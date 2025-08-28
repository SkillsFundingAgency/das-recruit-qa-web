using System.Collections.Generic;
using System.Linq;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Shared.Web.ViewModels.Qualifications;

namespace Recruit.Shared.Web.Extensions;

public static class QualificationsMapperExtensions
{
    public static IEnumerable<QualificationEditModel> ToViewModel(this IEnumerable<Qualification> qualifications)
    {
        if (qualifications == null)
        {
            return Enumerable.Empty<QualificationEditModel>();
        }

        return qualifications.Select(q => new QualificationEditModel
        {
            QualificationType = q.QualificationType,
            Subject = q.Subject,
            Grade = q.Grade,
            Weighting = q.Weighting
        });

    }
        
    public static IEnumerable<Qualification> ToEntity(this IEnumerable<QualificationEditModel> qualifications)
    {
        if (qualifications == null)
        {
            return Enumerable.Empty<Qualification>();
        }
            
        return qualifications.Select(q => new Qualification
        {
            QualificationType = q.QualificationType,
            Subject = q.Subject,
            Grade = q.Grade,
            Weighting = q.Weighting
        });
    }
}