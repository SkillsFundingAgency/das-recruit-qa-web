using System.Collections.Generic;

namespace Recruit.Shared.Web.ViewModels;

public class ReviewSummaryViewModel
{
    public bool HasBeenReviewed { get; internal set; }
    public string ReviewerComments { get; internal set; }
    public IEnumerable<ReviewFieldIndicatorViewModel> FieldIndicators { get; set; } = new List<ReviewFieldIndicatorViewModel>();
}