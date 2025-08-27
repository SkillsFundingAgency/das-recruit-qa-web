using Microsoft.AspNetCore.Http;

namespace Recruit.Shared.Web.Extensions;

public static class RequestExtensions
{
    public static string GetRequestUrlRoot(this HttpRequest request)
    {
        var url = $"{request.Scheme}://{request.Host}";
        return url;
    }
}