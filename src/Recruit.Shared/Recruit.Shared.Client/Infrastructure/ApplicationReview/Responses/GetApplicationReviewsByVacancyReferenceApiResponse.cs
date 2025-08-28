#nullable enable
using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.ApplicationReview.Responses;

public record GetApplicationReviewsByVacancyReferenceApiResponse
{
    public List<ApplicationReview> ApplicationReviews { get; init; } = [];
}