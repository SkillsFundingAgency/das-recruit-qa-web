using Recruit.Vacancies.Client.Application.Providers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Recruit.Qa.Web.Configuration;
using Recruit.Qa.Web.Configuration.Routing;

namespace Recruit.Qa.Web.Controllers;

public class CookieManagerController(IWebHostEnvironment hostingEnvironment, ITimeProvider timeProvider)
    : Controller
{
    [HttpPost("dismiss-outage-message", Name = RouteNames.DismissOutageMessage_Post)]
    public IActionResult DismissOutageMessage([FromForm]string returnUrl)
    {
        const string SeenCookieValue = "1";

        if (!Request.Cookies.ContainsKey(CookieNames.SeenOutageMessage))
            Response.Cookies.Append(CookieNames.SeenOutageMessage, SeenCookieValue, EsfaCookieOptions.GetSingleDayLifetimeHttpCookieOption(hostingEnvironment, timeProvider));

        return RedirectToRoute(RouteNames.Dashboard_Index_Get);
    }
}