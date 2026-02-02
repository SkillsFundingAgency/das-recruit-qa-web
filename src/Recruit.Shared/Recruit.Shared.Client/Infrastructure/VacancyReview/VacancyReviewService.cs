using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Requests;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview.Responses;
using SFA.DAS.Encoding;

namespace Recruit.Vacancies.Client.Infrastructure.VacancyReview;


public interface IVacancyReviewRepositoryRunner
{
    Task UpdateAsync(Domain.Entities.VacancyReview review);
}

public class VacancyReviewRepositoryRunner(IEnumerable<IVacancyReviewRepository> reviewResolver)
    : IVacancyReviewRepositoryRunner
{
    public async Task UpdateAsync(Domain.Entities.VacancyReview vacancyReview)
    {
        foreach (var vacancyReviewResolver in reviewResolver)
        {
            await vacancyReviewResolver.UpdateAsync(vacancyReview);
        }
    }
}

public class VacancyReviewService(IRecruitQaOuterApiClient outerApiClient, IEncodingService encodingService) : IVacancyReviewRepository, IVacancyReviewQuery
{
    public string Key { get; } = "OuterApiVacancyReview";
    
    public async Task<Domain.Entities.VacancyReview> GetAsync(Guid reviewId)
    {
        var result = await outerApiClient.Get<GetVacancyReviewResponse>(new GetVacancyReviewRequest(reviewId));

        if (result == null)
        {
            return null;
        }
        
        return (Domain.Entities.VacancyReview)result.VacancyReview;
    }

    public async Task UpdateAsync(Domain.Entities.VacancyReview review)
    {
        await outerApiClient.Post(new PostVacancyReviewRequest(review.Id,VacancyReviewDto.MapVacancyReviewDto(review, encodingService)), false);
    }

    public async Task<List<Domain.Entities.VacancyReview>> GetForVacancyAsync(long vacancyReference)
    {
        var result = await outerApiClient.Get<GetVacancyReviewListResponse>(
            new GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(vacancyReference, [ManualQaOutcome.Approved, ManualQaOutcome.Referred, ManualQaOutcome.Blocked], false));
        return result.VacancyReviews.Select(c=>(Domain.Entities.VacancyReview)c).ToList();
    }

    public async Task<Domain.Entities.VacancyReview> GetLatestReviewByReferenceAsync(long vacancyReference)
    {
        var result = await outerApiClient.Get<GetVacancyReviewListResponse>(new GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(vacancyReference,
            [ManualQaOutcome.Approved, ManualQaOutcome.Referred], true));
        
        if (result == null)
        {
            return null;
        }
        
        return (Domain.Entities.VacancyReview)result.VacancyReviews.FirstOrDefault();
    }
    
    public async Task<GetVacancyReviewSummaryResponse> GetVacancyReviewSummary()
    {
        return await outerApiClient.Get<GetVacancyReviewSummaryResponse>(new GetVacancyReviewSummaryRequest());
    }

    public async Task<List<Domain.Entities.VacancyReview>> GetByStatusAsync(ReviewStatus status)
    {
        var result = await outerApiClient.Get<GetVacancyReviewListResponse>(new GetVacancyReviewByFilterRequest([status]));
        return result.VacancyReviews.Select(c=>(Domain.Entities.VacancyReview)c).ToList();
    }

    public async Task<List<Domain.Entities.VacancyReview>> GetVacancyReviewsInProgressAsync(DateTime getExpiredAssignationDateTime)
    {
        var result = await outerApiClient.Get<GetVacancyReviewListResponse>(new GetVacancyReviewByFilterRequest(
            [ReviewStatus.UnderReview], expiredAssignationDateTime:getExpiredAssignationDateTime));
        return result.VacancyReviews.Select(c=>(Domain.Entities.VacancyReview)c).ToList();
    }

    public async Task<int> GetApprovedCountAsync(string submittedByUserEmail)
    {
        var result = await outerApiClient.Get<GetVacancyReviewCountResponse>(new GetVacancyReviewCountByUserFilterRequest(submittedByUserEmail));
        return result.Count;
    }

    public async Task<int> GetApprovedFirstTimeCountAsync(string submittedByUserEmail)
    {
        var result = await outerApiClient.Get<GetVacancyReviewCountResponse>(new GetVacancyReviewCountByUserFilterRequest(submittedByUserEmail, true));
        return result.Count;
    }

    public async Task<List<Domain.Entities.VacancyReview>> GetAssignedForUserAsync(string userId, DateTime assignationExpiryDateTime)
    {
        var result =
            await outerApiClient.Get<GetVacancyReviewListResponse>(
                new GetVacancyReviewsAssignedToUserRequest(userId, assignationExpiryDateTime, nameof(ReviewStatus.UnderReview)));
        return result.VacancyReviews.Select(c=>(Domain.Entities.VacancyReview)c).ToList();
    }

    public async Task<Domain.Entities.VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference)
    {
        var result = await outerApiClient.Get<GetVacancyReviewListResponse>(
            new GetVacancyReviewByVacancyReferenceAndReviewStatusRequest(vacancyReference,
                [ManualQaOutcome.Referred], false, nameof(ReviewStatus.Closed)));
        
        if (result == null)
        {
            return null;
        }
        
        return (Domain.Entities.VacancyReview)result.VacancyReviews.OrderByDescending(r => r.ClosedDate).FirstOrDefault();
    }

    public async Task<int> GetAnonymousApprovedCountAsync(string accountLegalEntityPublicHashedId)
    {
        var accountLegalEntity =
            encodingService.Decode(accountLegalEntityPublicHashedId, EncodingType.PublicAccountLegalEntityId);
        // this is just used as a flag so can just return 1 or zero
        var result = await outerApiClient.Get<GetVacancyReviewCountResponse>(new GetAnonymousApprovedCountByAccountLegalEntity(accountLegalEntity));
        return result.Count;
    }
}