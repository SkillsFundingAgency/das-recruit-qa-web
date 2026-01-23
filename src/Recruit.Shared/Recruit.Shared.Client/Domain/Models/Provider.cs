using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using static Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses.GetProviderApiResponse;

namespace Recruit.Vacancies.Client.Domain.Models;

public record Provider
{
    public int Ukprn { get; set; }
    public string Name { get; set; }
    public string ContactUrl { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int ProviderTypeId { get; set; }
    public int StatusId { get; set; }
    public GetProviderAddress Address { get; set; }

    public static implicit operator Provider(GetProviderApiResponse source)
    {
        return new Provider
        {
            Ukprn = source.Ukprn,
            Name = source.Name,
            ContactUrl = source.ContactUrl,
            Email = source.Email,
            Phone = source.Phone,
            ProviderTypeId = source.ProviderTypeId,
            StatusId = source.StatusId,
            Address = source.Address
        };
    }
}
