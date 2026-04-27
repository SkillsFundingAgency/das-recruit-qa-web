using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruit.Qa.Web.Configuration.Routing;
using Recruit.Qa.Web.Orchestrators.Reports;
using Recruit.Qa.Web.Security;

namespace Recruit.Qa.Web.Controllers.Reports;

[Authorize(Policy = AuthorizationPolicyNames.TeamLeadUserPolicyName)]
public class ReportDashboardController(ReportDashboardOrchestrator orchestrator) : Controller
{
    [HttpGet(RoutePaths.ReportsDashboardRoutePath, Name = RouteNames.ReportDashboard_Get)]
    public async Task<IActionResult> Dashboard()
    {
        var vm = await orchestrator.GetDashboardViewModel();
        if (TempData.TryGetValue("NewReportId", out var newReportIdObj) &&
            Guid.TryParse(newReportIdObj?.ToString(), out var newReportId))
        {
            var newReport = vm.Reports?.FirstOrDefault(r => r.ReportId == newReportId);
            if (newReport != null)
            {
                vm.ShowSuccessBanner = true;
                vm.SuccessReportName = newReport.ReportName;
                TempData.Remove("NewReportId");
            }
        }
        return View(vm);
    }

    [HttpGet(RoutePaths.ReportDownloadCsvRoutePath, Name = RouteNames.ReportDashboard_DownloadCsv)]
    public async Task<IActionResult> DownloadCsv([FromRoute] Guid reportId)
    {
        var vm = await orchestrator.GetDownloadCsvAsync(reportId);
        return File(vm.Content, "application/octet-stream", $"{ToValidFileName(vm.ReportName)}.csv");
    }

    private string ToValidFileName(string text)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            text = text.Replace(c, '-');
        }

        return text;
    }
}