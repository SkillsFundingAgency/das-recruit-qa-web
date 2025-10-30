using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Recruit.Vacancies.Client.Application.Providers;

namespace Recruit.Qa.Web.Configuration;

public class EsfaCookieOptions
{
    public static CookieOptions GetSingleDayLifetimeHttpCookieOption(IWebHostEnvironment env, ITimeProvider timeProvider) => new CookieOptions
    {
        Secure = !env.IsDevelopment(),
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        Expires = timeProvider.NextDay
    };
}