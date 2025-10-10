using System;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Services.NextVacancyReview;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class AssignVacancyReviewCommandHandler(
    ILogger<AssignVacancyReviewCommand> logger,
    IVacancyReviewRepositoryRunner vacancyReviewRepositoryRunner,
    ITimeProvider timeProvider,
    INextVacancyReviewService nextVacancyReviewService,
    IVacancyReviewQuery vacancyReviewQuery)
    : IRequestHandler<AssignVacancyReviewCommand, Unit>
{
    public async Task<Unit> Handle(AssignVacancyReviewCommand message, CancellationToken cancellationToken)
    {
        VacancyReview review;

        if (message.ReviewId.HasValue)
        {
            logger.LogInformation("Starting assignment of review for user {userId} for review {reviewId}", message.User.UserId, message.ReviewId);
            review = await GetVacancyReviewAsync(message.ReviewId.Value);
        }
        else
        {
            logger.LogInformation("Starting assignment of next review for user {userId}.", message.User.UserId);
            review = await GetNextVacancyReviewForUser(message.User.UserId);
        }

        if (review == null)
            return Unit.Value;

        review.Status = ReviewStatus.UnderReview;
        review.ReviewedByUser = message.User;
        review.ReviewedDate = timeProvider.Now;

        await vacancyReviewRepositoryRunner.UpdateAsync(review);
        return Unit.Value;
    }

    private async Task<VacancyReview> GetVacancyReviewAsync(Guid reviewId)
    {
        var review = await vacancyReviewQuery.GetAsync(reviewId);

        if (nextVacancyReviewService.VacancyReviewCanBeAssigned(review.Status, review.ReviewedDate))
            return review;

        logger.LogWarning($"Unable to assign review {{reviewId}} for vacancy {{vacancyReference}} due to review having a status of {review.Status}.", review.Id, review.VacancyReference);
        return null;
    }

    private async Task<VacancyReview> GetNextVacancyReviewForUser(string userId)
    {
        var review = await nextVacancyReviewService.GetNextVacancyReviewAsync(userId);

        if (review == null)
        {
            logger.LogInformation("No reviews to assign to user {userId}.", userId);
        }

        return review;
    }
}