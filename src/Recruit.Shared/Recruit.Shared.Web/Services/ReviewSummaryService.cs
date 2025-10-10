using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Shared.Web.Mappers;
using Recruit.Shared.Web.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Recruit.Shared.Web.Services;

public class ReviewSummaryService(
    ReviewFieldIndicatorMapper fieldMappingsLookup,
    IQaVacancyClient qaVacancyClient)
    : IReviewSummaryService
{
    public async Task<ReviewSummaryViewModel> GetReviewSummaryViewModelAsync(Guid reviewId,
        ReviewFieldMappingLookupsForPage reviewFieldIndicatorsForPage)
    {
        var review = await qaVacancyClient.GetVacancyReviewAsync(reviewId);

        return ConvertToReviewSummaryViewModel(reviewFieldIndicatorsForPage, review);
    }

    private ReviewSummaryViewModel ConvertToReviewSummaryViewModel(
        ReviewFieldMappingLookupsForPage reviewFieldIndicatorsForPage, VacancyReview review)
    {
        if (review != null)
        {
            var fieldIndicators =
                fieldMappingsLookup.MapFromFieldIndicators(reviewFieldIndicatorsForPage, review).ToList();

            return new ReviewSummaryViewModel
            {
                HasBeenReviewed = true,
                ReviewerComments = review.ManualQaComment,
                FieldIndicators = fieldIndicators
            };
        }

        return new ReviewSummaryViewModel();
    }
}