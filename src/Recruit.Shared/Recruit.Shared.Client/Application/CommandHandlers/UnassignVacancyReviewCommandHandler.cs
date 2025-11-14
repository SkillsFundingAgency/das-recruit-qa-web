using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class UnassignVacancyReviewCommandHandler(
    IVacancyReviewRepositoryRunner repository,
    IVacancyReviewQuery vacancyReviewQuery,
    ILogger<UnassignVacancyReviewCommandHandler> logger)
    : IRequestHandler<UnassignVacancyReviewCommand, Unit>
{
    public async Task<Unit> Handle(UnassignVacancyReviewCommand message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to unassign review {reviewId}.", message.ReviewId);

        var review = await vacancyReviewQuery.GetAsync(message.ReviewId);

        if (!review.CanUnassign)
        {
            logger.LogWarning($"Unable to unassign {review.ReviewedByUser.Name} from review {message.ReviewId}, it may already be unassigned.");
            return Unit.Value;
        }

        review.Status = ReviewStatus.PendingReview;
        review.ReviewedDate = null;
        review.ReviewedByUser = null;

        await repository.UpdateAsync(review);
            
        return Unit.Value;
    }
}