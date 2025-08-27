using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recruit.Qa.Web.ViewModels.Home;

namespace Recruit.Qa.Web.Controllers;

[Route("")]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        bool isDfESignInAllowed = _configuration.GetValue<bool>("UseDfESignIn");

        // if the DfESignIn toggle off or user is authenticated, then redirect the user to dashboard.
        if (!isDfESignInAllowed || User.Identity is { IsAuthenticated: true }) return RedirectToAction("Index", "Dashboard");

        return View(new HomeIndexViewModel
        {
            UseDfESignIn = isDfESignInAllowed
        });
    }
}