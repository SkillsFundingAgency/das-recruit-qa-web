using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Validation.Fluent;
using FluentValidation;
using Recruit.Shared.Web.ViewModels.ApplicationReviews;

namespace Recruit.Shared.Web.ViewModels.Validations.Fluent;

public class ApplicationReviewsFeedbackModelValidator : AbstractValidator<IApplicationReviewsEditModel>
{
    public ApplicationReviewsFeedbackModelValidator(IProfanityListProvider profanityListProvider)
    {
        RuleFor(x => x.CandidateFeedback)
            .NotEmpty()
            .WithMessage(x => x.IsMultipleApplications
                ? ApplicationReviewValidator.CandidateFeedbackRequiredForMultipleApplications
                : ApplicationReviewValidator.CandidateFeedbackRequiredForSingleApplication)
            .MaximumLength(ApplicationReviewValidator.CandidateFeedbackMaxLength)
            .WithMessage(string.Format(ApplicationReviewValidator.CandidateFeedbackLength, ApplicationReviewValidator.CandidateFeedbackMaxLength))
            .Must(ApplicationReviewValidator.BeWithinMaxWordsOrEmpty)
            .WithMessage(string.Format(ApplicationReviewValidator.CandidateFeedbackWordsLength, ApplicationReviewValidator.CandidateFeedbackMaxWordLength))
            .ValidFreeTextCharacters()
            .WithMessage(ApplicationReviewValidator.CandidateFeedbackFreeTextCharacters)
            .ProfanityCheck(profanityListProvider)
            .WithMessage(ApplicationReviewValidator.CandidateFeedbackProfanityPhrases)
            .WithErrorCode("617");
    }
}