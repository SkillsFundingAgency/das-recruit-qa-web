using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Domain.Entities;

public class GetAddressesRequest(string query) : IGetApiRequest
{
    string IGetApiRequest.GetUrl => $"locations?query={query}";
      
}