using System.Diagnostics;
using System.Net;
using Recruit.Vacancies.Client.Domain.Exceptions;
using Recruit.Vacancies.Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Recruit.Qa.Web.Configuration;
using Recruit.Qa.Web.Configuration.Routing;
using Recruit.Qa.Web.Exceptions;
using Recruit.Qa.Web.Models.Error;
using Recruit.Qa.Web.ViewModels;

namespace Recruit.Qa.Web.Controllers;

[AllowAnonymous]
public class ErrorController(ILogger<ErrorController> logger, IConfiguration configuration)
    : Controller
{
    [Route("error/{id?}")]
    public IActionResult Error(int id)
    {
        switch (id)
        {
            case 403:
                return AccessDenied();
            case 404:
                return PageNotFound();
        }

        return View(new ErrorViewModel { StatusCode = id, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route(RoutePaths.ExceptionHandlingPath)]
    public IActionResult ErrorHandler()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature != null)
        {
            var exception = exceptionFeature.Error;

            switch (exception)
            {
                case NotFoundException _:
                    logger.LogError(exception, "Exception on path: {route}", exceptionFeature.Path);
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return View(ViewNames.ErrorView, GetViewModel(HttpStatusCode.NotFound));

                case ReportNotFoundException _:
                case VacancyNotFoundException _:
                    return PageNotFound();

                case UnassignedVacancyReviewException _:
                case InvalidStateException _:
                    TempData.Add(TempDataKeys.DashboardMessage, exception.Message);
                    return RedirectToRoute(RouteNames.Dashboard_Index_Get);

                default:
                    break;
            }

            logger.LogError(exception, "Unhandled exception on path: {route}", exceptionFeature.Path);
        }

        return View(ViewNames.ErrorView, GetViewModel(HttpStatusCode.InternalServerError));
    }

    private ErrorViewModel GetViewModel(HttpStatusCode statusCode)
    {
        return new ErrorViewModel { StatusCode = (int)statusCode, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
    }

    private IActionResult AccessDenied()
    {
        bool isDfESignInAllowed = configuration.GetValue<bool>("UseDfESignIn");
        Response.StatusCode = (int) HttpStatusCode.Forbidden;
        return View(ViewNames.AccessDenied, new Error403ViewModel(configuration.GetValue<string>("ResourceEnvironmentName"))
        {
            UseDfESignIn = isDfESignInAllowed
        });
    }

    private IActionResult PageNotFound()
    {
        Response.StatusCode = (int)HttpStatusCode.NotFound;
        return View(ViewNames.PageNotFound);
    }
}