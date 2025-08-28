using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.HttpRequestHandlers;

public class VersionHeaderHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Version", "1.0");
        return base.SendAsync(request, cancellationToken);
    }
}
