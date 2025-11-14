using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recruit.Qa.Web.ViewModels.Home;

namespace Recruit.Qa.Web.Controllers;

[Route("")]
[Route("[controller]")]
public class HomeController(IConfiguration configuration) : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        bool isDfESignInAllowed = configuration.GetValue<bool>("UseDfESignIn");

        // if the DfESignIn toggle off or user is authenticated, then redirect the user to dashboard.
        if (!isDfESignInAllowed || User.Identity is { IsAuthenticated: true }) return RedirectToAction("Index", "Dashboard");

        return View(new HomeIndexViewModel
        {
            UseDfESignIn = isDfESignInAllowed
        });
    }
}