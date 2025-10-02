using FluentValidation;
using Recruit.Qa.Web.ViewModels.ManageProvider;

namespace Recruit.Qa.Web.ViewModels.ManageProvider.Validations;

public class ConsentForProviderBlockingEditModelValidator : AbstractValidator<ConsentForProviderBlockingEditModel>
{
    public ConsentForProviderBlockingEditModelValidator()
    {
        RuleFor(m => m.HasConsent)
            .Equal(true)
            .WithMessage("Please tick the box to continue");
        RuleFor(m => m.Reason)
            .NotEmpty()
            .When(m => m.HasConsent)
            .WithMessage("Please enter a reason");
    }
}