using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.Services;

public class VacancyReviewTransferService : IVacancyReviewTransferService
{
    private readonly ILogger<VacancyReviewTransferService> _logger;
    private readonly IVacancyReviewQuery _vacancyReviewQuery;
    private readonly IVacancyReviewRepositoryRunner _vacancyReviewRepository;
    private readonly ITimeProvider _timeProvider;

    public VacancyReviewTransferService(ILogger<VacancyReviewTransferService> logger, IVacancyReviewQuery vacancyReviewQuery,
        IVacancyReviewRepositoryRunner vacancyReviewRepository, ITimeProvider timeProvider)
    {
        _logger = logger;
        _vacancyReviewQuery = vacancyReviewQuery;
        _vacancyReviewRepository = vacancyReviewRepository;
        _timeProvider = timeProvider;
    }

    public async Task CloseVacancyReview(long vacancyReference, TransferReason transferReason)
    {
        var review = await _vacancyReviewQuery.GetLatestReviewByReferenceAsync(vacancyReference);

        if (review.IsPending)
        {
            review.ManualOutcome = transferReason == TransferReason.BlockedByQa
                ? ManualQaOutcome.Blocked
                : ManualQaOutcome.Transferred;
            review.Status = ReviewStatus.Closed;
            review.ClosedDate = _timeProvider.Now;

            await _vacancyReviewRepository.UpdateAsync(review);
        }
        else if (review.Status == ReviewStatus.UnderReview)
        {
            _logger.LogWarning($"Latest review for vacancy {review.VacancyReference} that has been transferred is currently being reviewed.");
        }
    }
}