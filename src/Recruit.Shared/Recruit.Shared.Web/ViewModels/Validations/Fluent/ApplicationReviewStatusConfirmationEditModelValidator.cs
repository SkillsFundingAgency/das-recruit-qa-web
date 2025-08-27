using Esfa.Recruit.Vacancies.Client.Application.Validation.Fluent;
using FluentValidation;
using Recruit.Shared.Web.ViewModels.ApplicationReview;

namespace Recruit.Shared.Web.ViewModels.Validations.Fluent;

public class ApplicationReviewStatusConfirmationEditModelValidator : AbstractValidator<IApplicationStatusConfirmationEditViewModel>
{
    public ApplicationReviewStatusConfirmationEditModelValidator()
    {
        RuleFor(x => x.NotifyCandidate)
            .NotNull()
            .WithMessage(ApplicationReviewValidator.NotifyCandidateRequired);                             
    }
}