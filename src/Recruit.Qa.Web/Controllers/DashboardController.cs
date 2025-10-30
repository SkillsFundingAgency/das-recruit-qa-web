using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recruit.Qa.Web.Configuration;
using Recruit.Qa.Web.Configuration.Routing;
using Recruit.Qa.Web.Extensions;
using Recruit.Qa.Web.Orchestrators;

namespace Recruit.Qa.Web.Controllers;

[Route("[controller]")]
public class DashboardController(DashboardOrchestrator orchestrator, IConfiguration configuration)
    : Controller
{
    [HttpGet]
    [Route("",Name = RouteNames.Dashboard_Index_Get)]
    public async Task<IActionResult> Index([FromQuery]string searchTerm)
    {
        bool isDfESignInAllowed = configuration.GetValue<bool>("UseDfESignIn");
        if (isDfESignInAllowed && User.Identity is { IsAuthenticated: false })
        {
            // if the user is not authenticated, redirect them back to start now page.
            return RedirectToAction("Index", "Home");
        }

        var vm = await orchestrator.GetDashboardViewModelAsync(searchTerm, User);

        vm.DashboardMessage = TempData[TempDataKeys.DashboardMessage]?.ToString();
            
        return View(vm);
    }

    [HttpPost]
    [Route("next-vacancy",Name = RouteNames.Dashboard_Next_Vacancy_Post)]
    public async Task<IActionResult> NextVacancy()
    {
        var vacancyReviewId = await orchestrator.AssignNextVacancyReviewAsync(User.GetVacancyUser());

        if (vacancyReviewId == null)
            return RedirectToRoute(RouteNames.Dashboard_Index_Get);

        return RedirectToRoute(RouteNames.Vacancy_Review_Get, new {reviewId = vacancyReviewId});
    }

    [HttpGet]
    [Route("next-vacancy",Name = RouteNames.Dashboard_Next_Vacancy_Get)]
    public IActionResult NextVacancyCallback()
    {
        //This GET handles the authentication callback when NextVacancy is POSTed after a session timeout
        return RedirectToRoute(RouteNames.Dashboard_Index_Get);
    }
}