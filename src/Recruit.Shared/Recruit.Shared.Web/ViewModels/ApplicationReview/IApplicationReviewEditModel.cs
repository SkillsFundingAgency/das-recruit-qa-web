using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Shared.Web.ViewModels.ApplicationReview;

public interface IApplicationReviewEditModel
{
    ApplicationReviewStatus? Outcome { get; set; }
    string CandidateFeedback { get; set; }
    bool NavigateToFeedbackPage { get; set; }
}