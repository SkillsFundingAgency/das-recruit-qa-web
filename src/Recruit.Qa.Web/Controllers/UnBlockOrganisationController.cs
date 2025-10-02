using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Recruit.Qa.Web.Configuration.Routing;
using Recruit.Qa.Web.Extensions;
using Recruit.Qa.Web.Security;
using Recruit.Qa.Web.Configuration;
using Recruit.Qa.Web.ViewModels.ManageProvider;
using Recruit.Qa.Web.Orchestrators;

namespace Recruit.Qa.Web.Controllers;

[Authorize(Policy = AuthorizationPolicyNames.TeamLeadUserPolicyName)]
[Route(RoutePaths.BlockedOrganisations)]
public class UnblockOrganisationController : Controller
{
    private readonly UnblockOrganisationOrchestrator _orchestrator;

    public UnblockOrganisationController(UnblockOrganisationOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpGet("unblock", Name = RouteNames.UnBlockProvider_Confirm_Get)]
    public async Task<IActionResult> ConfirmTrainingProviderUnblocking(string organisationId)
    {
        if (long.TryParse(organisationId, out var ukprn) == false) return BadRequest();
        var vm = await _orchestrator.GetConfirmTrainingProviderUnblockingViewModel(ukprn);
        return View(vm);
    }

    [HttpPost("unblock", Name = RouteNames.UnBlockProvider_Confirm_Post)]
    public async Task<IActionResult> ConfirmTrainingProviderUnblocking(ConfirmTrainingProviderUnblockingEditModel model)
    {
        if (ModelState.IsValid == false)
        {
            return View(new ConfirmTrainingProviderUnblockingEditModel() { Ukprn = model.Ukprn, ProviderName = model.ProviderName });
        }
        var isBlocked = await _orchestrator.IsProviderAlreadyBlocked(model.Ukprn);

        if (isBlocked && model.CanRestoreAccess)
        {
            TempData[TempDataKeys.UnBlockedProviderUkprnKey] = model.Ukprn;
            await _orchestrator.UnblockProviderAsync(model.Ukprn, User.GetVacancyUser());
            return RedirectToRoute(RouteNames.UnBlockProvider_Acknowledgement_Get);
        }
        return RedirectToRoute(RouteNames.BlockedOrganisations_Get);
    }

    [HttpGet("unblocking-acknowledgement", Name = RouteNames.UnBlockProvider_Acknowledgement_Get)]
    public async Task<IActionResult> ProviderUnBlockedAcknowledgement()
    {
        var data = TempData[TempDataKeys.UnBlockedProviderUkprnKey]?.ToString();
        if (long.TryParse(data, out var ukprn) == false) return BadRequest();
        var vm = await _orchestrator.GetAcknowledgementViewModelAsync(ukprn);
        return View(vm);
    }
}