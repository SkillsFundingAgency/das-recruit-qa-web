using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

public interface IOuterApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
    Task Post(IPostApiRequest request, bool ensureSuccessStatusCode = true);
    Task<TResponse> Post<TResponse>(IPostApiRequest request);
}

public interface IGetApiRequest
{
    string GetUrl { get; }
}

public interface IPostApiRequest
{
    [JsonIgnore]
    string PostUrl { get; }
    object Data { get; set; }
}