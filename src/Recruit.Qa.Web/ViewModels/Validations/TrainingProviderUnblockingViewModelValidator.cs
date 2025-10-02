using FluentValidation;
using Recruit.Qa.Web.ViewModels.ManageProvider;

namespace Recruit.Qa.Web.ViewModels.Validations;

public class TrainingProviderUnblockingViewModelValidator : AbstractValidator<ConfirmTrainingProviderUnblockingEditModel>
{
    public const string SelectionRequired = "Please select an option to confirm";
    public TrainingProviderUnblockingViewModelValidator()
    {
        RuleFor(x => x.RestoreAccess)
            .NotNull()
            .WithMessage(SelectionRequired);

    }
}