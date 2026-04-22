using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Events;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class ApproveVacancyReviewCommandHandler(
    ILogger<ApproveVacancyReviewCommandHandler> logger,
    IVacancyReviewRepositoryRunner vacancyReviewRepositoryRunner,
    IVacancyReviewQuery vacancyReviewQuery,
    IVacancyRepository vacancyRepository,
    IMessaging messaging,
    IValidator<VacancyReview> vacancyReviewValidator,
    ITimeProvider timeProvider)
    : IRequestHandler<ApproveVacancyReviewCommand, Unit>
{
    public async Task<Unit> Handle(ApproveVacancyReviewCommand message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Approving review {reviewId}.", message.ReviewId);

        var review = await vacancyReviewQuery.GetAsync(message.ReviewId);
        var vacancy = await vacancyRepository.GetVacancyAsync(review.VacancyReference);

        var initialReviewStatus = review.Status;
        if (!review.CanApprove)
        {
            logger.LogWarning($"Unable to approve review {{reviewId}} due to review having a status of {review.Status}.", message.ReviewId);
            return Unit.Value;
        }

        review.ManualOutcome = ManualQaOutcome.Approved;
        review.Status = ReviewStatus.Closed;
        review.ClosedDate = timeProvider.Now;
        review.ManualQaComment = message.ManualQaComment;
        review.ManualQaFieldIndicators = message.ManualQaFieldIndicators;
        review.ManualQaFieldEditIndicators = message.ManualQaFieldEditIndicators;
        foreach (var automatedQaOutcomeIndicator in review.AutomatedQaOutcomeIndicators)
        {
            automatedQaOutcomeIndicator.IsReferred = message.SelectedAutomatedQaRuleOutcomeIds
                .Contains(automatedQaOutcomeIndicator.RuleOutcomeId);
        }

        Validate(review);

        var closureReason = await TryGetReasonToCloseVacancy(review, vacancy);
        if (closureReason != null)
        {
            // review should have been closed and vacancy transferred
            if (initialReviewStatus is not ReviewStatus.Closed)
            {
                review.ManualOutcome = ManualQaOutcome.Transferred;
                await vacancyReviewRepositoryRunner.UpdateAsync(review);        
            }
            
            return Unit.Value;
        }
        
        await vacancyReviewRepositoryRunner.UpdateAsync(review);
        await PublishVacancyReviewApprovedEventAsync(message, review);
        return Unit.Value;
    }

   private async Task<ClosureReason?> TryGetReasonToCloseVacancy(VacancyReview review, Vacancy vacancy)
    {
        if (HasVacancyBeenTransferredSinceReviewWasCreated(review, vacancy))
        {
            return vacancy.TransferInfo.Reason == TransferReason.EmployerRevokedPermission ? 
                ClosureReason.TransferredByEmployer : ClosureReason.TransferredByQa;
        }

        return null;
    }

    private void Validate(VacancyReview review)
    {
        var validationResult = vacancyReviewValidator.Validate(review);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }

    private bool HasVacancyBeenTransferredSinceReviewWasCreated(VacancyReview review, Vacancy vacancy)
    {
        return review.VacancySnapshot.TransferInfo == null && vacancy.TransferInfo != null;
    }

    private Task PublishVacancyReviewApprovedEventAsync(ApproveVacancyReviewCommand message, VacancyReview review)
    {
        return messaging.PublishEvent(new VacancyReviewApprovedEvent
        {
            ReviewId = message.ReviewId,
            VacancyReference = review.VacancyReference
        });
    }
}