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
using Recruit.Vacancies.Client.Application.Communications;
using Recruit.Vacancies.Client.Infrastructure.StorageQueue;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class ApproveVacancyReviewCommandHandler(
    ILogger<ApproveVacancyReviewCommandHandler> logger,
    IVacancyReviewRepositoryRunner vacancyReviewRepositoryRunner,
    IVacancyReviewQuery vacancyReviewQuery,
    IVacancyRepository vacancyRepository,
    IMessaging messaging,
    AbstractValidator<VacancyReview> vacancyReviewValidator,
    ITimeProvider timeProvider,
    IBlockedOrganisationQuery blockedOrganisationQuery,
    ICommunicationQueueService communicationQueueService)
    : IRequestHandler<ApproveVacancyReviewCommand, Unit>
{
    public async Task<Unit> Handle(ApproveVacancyReviewCommand message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Approving review {reviewId}.", message.ReviewId);

        var review = await vacancyReviewQuery.GetAsync(message.ReviewId);
        var vacancy = await vacancyRepository.GetVacancyAsync(review.VacancyReference);

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

        await vacancyReviewRepositoryRunner.UpdateAsync(review);

        var closureReason = await TryGetReasonToCloseVacancy(review, vacancy);

        if (closureReason != null)
        {
            await CloseVacancyAsync(vacancy, closureReason.Value);
            await SendNotificationToEmployerAsync(vacancy.TrainingProvider.Ukprn.GetValueOrDefault(), vacancy.EmployerAccountId);
            return Unit.Value;
        }

        await PublishVacancyReviewApprovedEventAsync(message, review);    
        return Unit.Value;
    }

    private Task SendNotificationToEmployerAsync(long ukprn, string employerAccountId)
    {
        var communicationRequest = CommunicationRequestFactory.GetProviderBlockedEmployerNotificationForLiveVacanciesRequest(ukprn, employerAccountId);

        return communicationQueueService.AddMessageAsync(communicationRequest);
    }

    private async Task<ClosureReason?> TryGetReasonToCloseVacancy(VacancyReview review, Vacancy vacancy)
    {
        if (HasVacancyBeenTransferredSinceReviewWasCreated(review, vacancy))
        {
            return vacancy.TransferInfo.Reason == TransferReason.EmployerRevokedPermission ? 
                ClosureReason.TransferredByEmployer : ClosureReason.TransferredByQa;
        }

        if(await HasProviderBeenBlockedSinceReviewWasCreatedAsync(vacancy))
            return ClosureReason.BlockedByQa;

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

    private async Task<bool> HasProviderBeenBlockedSinceReviewWasCreatedAsync(Vacancy vacancy)
    {
        var blockedProvider = await blockedOrganisationQuery.GetByOrganisationIdAsync(vacancy.TrainingProvider.Ukprn.ToString());
        return blockedProvider?.BlockedStatus == BlockedStatus.Blocked;
    }

    private Task CloseVacancyAsync(Vacancy vacancy, ClosureReason closureReason)
    {
        vacancy.Status = VacancyStatus.Closed;
        vacancy.ClosedDate = timeProvider.Now;
        vacancy.ClosedByUser = vacancy.TransferInfo?.TransferredByUser;
        vacancy.ClosureReason = closureReason;
        return vacancyRepository.UpdateAsync(vacancy);
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