
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Validation.Fluent.CustomValidators.VacancyValidators;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Repositories;
using FluentValidation;

namespace Recruit.Vacancies.Client.Application.Validation.Fluent;

internal class TrainingProviderValidator : AbstractValidator<TrainingProvider>
{
    private const int UkprnLength = 8;

    public TrainingProviderValidator(long ruleId, ITrainingProviderSummaryProvider trainingProviderSummaryProvider, IBlockedOrganisationQuery blockedOrganisationRepo)
    {
        RuleFor(tp => tp.Ukprn.ToString())
            .NotEmpty()
            .WithMessage("You must enter a training provider")
            .WithErrorCode(ErrorCodes.TrainingProviderUkprnNotEmpty)
            .WithState(_=>ruleId)
            .Length(UkprnLength)
            .WithMessage($"The UKPRN is {UkprnLength} digits")
            .WithErrorCode(ErrorCodes.TrainingProviderUkprnMustBeCorrectLength)
            .WithState(_ => ruleId);

        When(tp => tp.Ukprn.ToString().Length == UkprnLength, () =>
        {
            RuleFor(tp => tp)
                .TrainingProviderMustExistInRoatp(trainingProviderSummaryProvider)
                .TrainingProviderMustNotBeBlocked(blockedOrganisationRepo)
                .TrainingProviderMustBeMainOrEmployerProfile(trainingProviderSummaryProvider);
        });
    }
}