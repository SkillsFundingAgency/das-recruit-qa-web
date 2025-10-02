using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Domain.Entities;
using NUnit.Framework;
using Recruit.Shared.Web.Extensions;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Shared.Web.Extensions;

public class QualificationExtensions
{
    [Test, AutoData]
    public void Then_Other_Qualifications_For_FaaV2_Are_Mapped(string grade, int level, string subject, string otherQualificationName)
    {
        var qualification = new Qualification
        {
            Grade = grade,
            Level = level,
            Subject = subject,
            QualificationType = "Other",
            OtherQualificationName = otherQualificationName
        };

        var actual = new List<Qualification> { qualification }.AsText();

        actual.First().Should()
            .Be(
                $"{qualification.QualificationType} (Level {qualification.Level}) ({qualification.OtherQualificationName}) {qualification.Subject} (Grade {qualification.Grade}) {qualification.Weighting.GetDisplayName().ToLower()}");
    }
    
    [Test, AutoData]
    public void Then_Qualifications_For_FaaV2_Are_Mapped(string grade, string subject)
    {
        var qualification = new Qualification
        {
            Grade = grade,
            Subject = subject,
            QualificationType = "A Level"
        };

        var actual = new List<Qualification> { qualification }.AsText();

        actual.First().Should()
            .Be(
                $"{qualification.QualificationType} {qualification.Subject} (Grade {qualification.Grade}) {qualification.Weighting.GetDisplayName().ToLower()}");
    }
}