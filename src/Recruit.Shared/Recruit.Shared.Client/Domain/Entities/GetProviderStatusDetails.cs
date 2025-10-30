using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Domain.Entities;

public class GetProviderStatusDetails(long ukprn) : IGetApiRequest
{
    public string GetUrl => $"provideraccounts/{ukprn}";
}