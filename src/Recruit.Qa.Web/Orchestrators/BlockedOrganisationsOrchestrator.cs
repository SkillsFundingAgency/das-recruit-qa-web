using Recruit.Qa.Web.ViewModels.ManageProvider;
using Recruit.Shared.Web.Extensions;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Extensions;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruit.Qa.Web.Orchestrators;

public class BlockedOrganisationsOrchestrator(
    IBlockedOrganisationQuery blockedOrganisationQuery,
    IQueryStoreReader queryStore,
    IMessaging messaging,
    ITimeProvider timeProvider,
    ITrainingProviderService trainingProviderService)
{
    public async Task<ConfirmTrainingProviderBlockingViewModel> GetConfirmTrainingProviderBlockingViewModelAsync(long ukprn)
    {
        var providerDetailTask = trainingProviderService.GetProviderAsync(ukprn);
        var providerEditVacancyInfoTask = queryStore.GetProviderVacancyDataAsync(ukprn);
        await Task.WhenAll(providerEditVacancyInfoTask, providerDetailTask);
        var providerDetail = providerDetailTask.Result;             
        var providerVacancyInfo = providerEditVacancyInfoTask.Result;

        var permissionCount = 0;
        if(providerVacancyInfo?.Employers != null)
        {
            permissionCount = providerVacancyInfo.Employers.SelectMany(e => e.LegalEntities).Count();
        }
            
        return ConvertToConfirmViewModel(providerDetail, permissionCount);
    }
    public async Task<ConsentForProviderBlockingViewModel> GetConsentForProviderBlockingViewModelAsync(long ukprn)
    {
        var providerDetailTask = trainingProviderService.GetProviderAsync(ukprn);
        var providerEditVacancyInfoTask = queryStore.GetProviderVacancyDataAsync(ukprn);
        await Task.WhenAll(providerEditVacancyInfoTask, providerDetailTask);
        var providerDetail = providerDetailTask.Result;             
        var providerVacancyInfo = providerEditVacancyInfoTask.Result;

        var permissionCount = 0;
        if(providerVacancyInfo?.Employers != null)
        {
            permissionCount = providerVacancyInfo.Employers.SelectMany(e => e.LegalEntities).Count();
        }
            
        return ConvertToConsentViewModel(providerDetail, permissionCount);
    }

    public Task BlockProviderAsync(long ukprn, string reason, VacancyUser user)
    {
        var command = new BlockProviderCommand(ukprn, user, timeProvider.Now, reason);
        return messaging.SendCommandAsync(command);
    }

    public async Task<ProviderBlockedAcknowledgementViewModel> GetAcknowledgementViewModelAsync(long ukprn)
    {
        var providerDetail = await trainingProviderService.GetProviderAsync(ukprn);

        return new ProviderBlockedAcknowledgementViewModel 
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

    public async Task<ProviderAlreadyBlockedViewModel> GetProviderAlreadyBlockedViewModelAsync(long ukprn)
    {
        var provider = await trainingProviderService.GetProviderAsync(ukprn);
        return new ProviderAlreadyBlockedViewModel{ Ukprn = ukprn, Name = provider.Name };
    }

    public async Task<BlockedOrganisationsViewModel> GetBlockedOrganisationsViewModel()
    {
        var blockedProviders = await queryStore.GetBlockedProvidersAsync();

        if (blockedProviders?.Data == null) return new BlockedOrganisationsViewModel();

        var blockedOrganisationViewModels = new List<BlockedOrganisationViewModel>();

        foreach(var provider in blockedProviders.Data)
        {
            blockedOrganisationViewModels.Add(ConvertToBlockedOrganisationViewModel(provider));
        }

        foreach( var vm in blockedOrganisationViewModels)
        {
            var ukprn = long.Parse(vm.OrganisationId);
            var prov = await trainingProviderService.GetProviderAsync(ukprn);
            vm.OrganisationName = prov?.Name;
            vm.Postcode = prov?.Address?.Postcode;
            vm.Ukprn = prov?.Ukprn.ToString();
        }

        return new BlockedOrganisationsViewModel { BlockedOrganisations = blockedOrganisationViewModels };
    }

    private BlockedOrganisationViewModel ConvertToBlockedOrganisationViewModel(BlockedOrganisationSummary summary) 
    {
        return new BlockedOrganisationViewModel
        {
            OrganisationId = summary.BlockedOrganisationId,
            BlockedOn = summary.BlockedDate.ToUkTime().AsGdsDateTime(),
            BlockedBy = summary.BlockedByUser
        };
    }

    private ConfirmTrainingProviderBlockingViewModel ConvertToConfirmViewModel(TrainingProvider provider, int permissionCount)
    {
        return new ConfirmTrainingProviderBlockingViewModel
        {
            Ukprn = provider.Ukprn.GetValueOrDefault(),
            Name = provider.Name,
            Address = provider.Address.ToAddressString(),
            PermissionCount = permissionCount
        };
    }
    private ConsentForProviderBlockingViewModel ConvertToConsentViewModel(TrainingProvider provider, int permissionCount)
    {
        return new ConsentForProviderBlockingViewModel
        {
            Name = provider.Name,
            PermissionCount = permissionCount
        };
    }
}