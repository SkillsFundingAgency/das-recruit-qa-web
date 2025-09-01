using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.ApplicationReview.Requests;
public record GetApplicationReviewByIdApiRequest(Guid ApplicationReviewId) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            return $"applicationReviews/{ApplicationReviewId}";
        }
    }
}