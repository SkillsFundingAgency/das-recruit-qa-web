using System.Collections.Generic;
using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.VacancyReview.Requests;

public class WhenBuildingGetLatestVacancyReviewByVacancyReferenceRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(long vacancyReference, string reviewStatus, List<ManualQaOutcome> manualOutcome)
    {
        var actual = new GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(vacancyReference, manualOutcome,false, reviewStatus);
        
        actual.GetUrl.Should().Be($"vacancies/{vacancyReference}/VacancyReviews?status={reviewStatus}&manualOutcome={string.Join("&manualOutcome=", manualOutcome)}&includeNoStatus=False");
    }
}