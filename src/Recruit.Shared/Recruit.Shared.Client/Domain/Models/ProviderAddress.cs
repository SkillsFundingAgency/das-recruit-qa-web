using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

namespace Recruit.Vacancies.Client.Domain.Models;

public record ProviderAddress
{
    public string Address1 { get; init; }
    public string Address2 { get; init; }
    public string Address3 { get; init; }
    public string Address4 { get; init; }
    public string Town { get; init; }
    public string County { get; init; }
    public string Postcode { get; init; }

    public static implicit operator ProviderAddress(GetProviderApiResponse.GetProviderAddress? source)
    {
        if (source is null)
            return new ProviderAddress();

        return new ProviderAddress
        {
            Address1 = source.Address1,
            Address2 = source.Address2,
            Address3 = source.Address3,
            Address4 = source.Address4,
            Town = source.Town,
            Postcode = source.Postcode
        };
    }
}