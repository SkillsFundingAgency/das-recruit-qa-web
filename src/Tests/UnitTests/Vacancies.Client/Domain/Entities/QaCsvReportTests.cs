using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Domain.Entities;

public class QaCsvReportTests
{
    [Test, AutoData]
    public void ImplicitOperator_MapsAllPropertiesFromQaReport(QaReport qaReport)
    {
        QaCsvReport result = qaReport;

        result.VacancyTitle.Should().Be(qaReport.VacancyTitle);
        result.VacancyReference.Should().Be(qaReport.VacancyReference);
        result.SubmissionNumber.Should().Be(qaReport.SubmissionNumber);
        result.DateSubmitted.Should().Be(qaReport.DateSubmitted);
        result.SlaDeadline.Should().Be(qaReport.SlaDeadline);
        result.ReviewStarted.Should().Be(qaReport.ReviewStarted);
        result.ReviewCompleted.Should().Be(qaReport.ReviewCompleted);
        result.Outcome.Should().Be(qaReport.Outcome);
        result.SlaExceededByHours.Should().Be(qaReport.SlaExceededByHours);
        result.TimeTakenToReview.Should().Be(qaReport.TimeTakenToReview);
        result.NumberOfIssuesReported.Should().Be(qaReport.NumberOfIssuesReported);
        result.VacancySubmittedBy.Should().Be(qaReport.VacancySubmittedBy);
        result.VacancySubmittedByUser.Should().Be(qaReport.VacancySubmittedByUser);
        result.Employer.Should().Be(qaReport.Employer);
        result.DisplayName.Should().Be(qaReport.DisplayName);
        result.TrainingProvider.Should().Be(qaReport.TrainingProvider);
        result.VacancyPostcode.Should().Be(qaReport.VacancyPostcode);
        result.ReferredFields.Should().BeEquivalentTo(qaReport.ReferredFields);
        result.ReviewedBy.Should().Be(qaReport.ReviewedBy);
        result.ReviewerComment.Should().Be(qaReport.ReviewerComment);
        result.CourseName.Should().Be(qaReport.CourseName);
        result.CourseLevel.Should().Be(qaReport.CourseLevel);
    }
}
