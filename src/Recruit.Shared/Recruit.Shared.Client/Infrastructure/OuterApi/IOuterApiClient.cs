using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi;

public interface IOuterApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
    Task Post(IPostApiRequest request, bool ensureSuccessStatusCode = true);
    Task<TResponse> Post<TResponse>(IPostApiRequest request);
}