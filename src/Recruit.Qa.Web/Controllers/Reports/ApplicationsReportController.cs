using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruit.Qa.Web.Configuration.Routing;
using Recruit.Qa.Web.Extensions;
using Recruit.Qa.Web.Orchestrators.Reports;
using Recruit.Qa.Web.RouteModel;
using Recruit.Qa.Web.Security;
using Recruit.Qa.Web.ViewModels.Reports.ApplicationsReport;

namespace Recruit.Qa.Web.Controllers.Reports;

[Authorize(Policy = AuthorizationPolicyNames.TeamLeadUserPolicyName)]
[Route(RoutePaths.ApplicationsReportRoutePath)]
public class ApplicationsReportController(ApplicationsReportOrchestrator orchestrator) : Controller
{
    [HttpGet("create", Name = RouteNames.ApplicationsReportCreate_Get)]
    public IActionResult Create()
    {
        var vm = orchestrator.GetCreateViewModel();

        return View(vm);
    }

    [HttpPost("create", Name = RouteNames.ApplicationsReportCreate_Post)]
    public async Task<IActionResult> Create(ApplicationsReportCreateEditModel m)
    {
        if (ModelState.IsValid == false)
        {
            var vm = orchestrator.GetCreateViewModel(m);

            return View(vm);
        }

        var reportId = await orchestrator.PostCreateViewModelAsync(m, User.GetVacancyUser());

        return RedirectToRoute(RouteNames.ReportConfirmation_Get, new ReportRouteModel { ReportId = reportId });
    }
}