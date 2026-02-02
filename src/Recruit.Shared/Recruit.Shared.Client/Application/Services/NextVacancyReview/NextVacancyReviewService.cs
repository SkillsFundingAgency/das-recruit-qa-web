using System;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace Recruit.Vacancies.Client.Application.Services.NextVacancyReview;

public class NextVacancyReviewService(
    IOptions<NextVacancyReviewServiceConfiguration> config,
    ITimeProvider timeProvider,
    IVacancyReviewQuery vacancyReviewQuery)
    : INextVacancyReviewService
{
    private readonly NextVacancyReviewServiceConfiguration _config = config.Value;

    public async Task<VacancyReview> GetNextVacancyReviewAsync(string email)
    {
        var assignationExpiryTime = timeProvider.Now.AddMinutes(_config.VacancyReviewAssignationTimeoutMinutes * -1);

        var assignedReviews = await vacancyReviewQuery.GetByStatusAsync(ReviewStatus.UnderReview);

        //Get the oldest unexpired review assigned to the user
        var nextVacancyReview = assignedReviews.Where(r => 
                r.ReviewedByUser.Email == email
                && r.ReviewedDate >= assignationExpiryTime)
            .OrderBy(r => r.CreatedDate)
            .FirstOrDefault();

        if (nextVacancyReview != null)
            return nextVacancyReview;

        //Get the oldest expired assigned review
        nextVacancyReview = assignedReviews.Where(r =>
                r.ReviewedDate < assignationExpiryTime)
            .OrderBy(r => r.CreatedDate)
            .FirstOrDefault();

        if (nextVacancyReview != null)
            return nextVacancyReview;

        //Get the oldest unassigned review
        var pendingReviews = await vacancyReviewQuery.GetByStatusAsync(ReviewStatus.PendingReview);
        nextVacancyReview = pendingReviews
            .OrderBy(r => r.CreatedDate)
            .FirstOrDefault();

        return nextVacancyReview;
    }

    public DateTime GetExpiredAssignationDateTime()
    {
        return timeProvider.Now.AddMinutes(_config.VacancyReviewAssignationTimeoutMinutes * -1);
    }

    public bool VacancyReviewCanBeAssigned(ReviewStatus reviewStatus, DateTime? reviewedDate)
    {
        if (reviewStatus == ReviewStatus.PendingReview)
            return true;

        return reviewedDate < GetExpiredAssignationDateTime();
    }
}