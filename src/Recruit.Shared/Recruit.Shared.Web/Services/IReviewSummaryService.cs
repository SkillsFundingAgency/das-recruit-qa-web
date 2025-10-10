using Recruit.Shared.Web.Mappers;
using Recruit.Shared.Web.ViewModels;
using System;
using System.Threading.Tasks;

namespace Recruit.Shared.Web.Services;

public interface IReviewSummaryService
{
    Task<ReviewSummaryViewModel> GetReviewSummaryViewModelAsync(Guid reviewId,
        ReviewFieldMappingLookupsForPage reviewFieldIndicatorsForPage);
}