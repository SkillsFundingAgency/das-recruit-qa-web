using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using Recruit.Qa.Web.ViewModels.ManageProvider;

namespace Recruit.Qa.Web.Orchestrators;

public class UnblockOrganisationOrchestrator(
    IBlockedOrganisationQuery blockedOrganisationQuery,
    IMessaging messaging,
    ITimeProvider timeProvider,
    ITrainingProviderService trainingProviderService)
{
    public async Task<ProviderUnblockedAcknowledgementViewModel> GetAcknowledgementViewModelAsync(long ukprn)
    {
        var providerDetail = await trainingProviderService.GetProviderAsync(ukprn);

        return new ProviderUnblockedAcknowledgementViewModel
        {
            Name = providerDetail.Name,
            Ukprn = ukprn
        };
    }

    public async Task<bool> IsProviderAlreadyBlocked(long ukprn)
    {
        var blockedOrganisation = await blockedOrganisationQuery.GetByOrganisationIdAsync(ukprn.ToString());
        return blockedOrganisation?.BlockedStatus == BlockedStatus.Blocked;
    }

    public Task UnblockProviderAsync(long ukprn, VacancyUser user)
    {
        var command = new UnblockProviderCommand(ukprn, user, timeProvider.Now);
        return messaging.SendCommandAsync(command);
    }

    public async Task<ConfirmTrainingProviderUnblockingEditModel> GetConfirmTrainingProviderUnblockingViewModel(long ukprn)
    {
        var provider = await trainingProviderService.GetProviderAsync(ukprn);
        return ConvertToConfirmViewModel(provider);
    }

    private ConfirmTrainingProviderUnblockingEditModel ConvertToConfirmViewModel(TrainingProvider provider)
    {
        return new ConfirmTrainingProviderUnblockingEditModel
        {
            Ukprn = provider.Ukprn.GetValueOrDefault(),
            ProviderName = provider.Name
        };
    }
}