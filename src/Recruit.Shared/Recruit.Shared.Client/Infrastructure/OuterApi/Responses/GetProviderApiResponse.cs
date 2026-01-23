namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;

public record GetProviderApiResponse
{
    public int Ukprn { get; set; }
    public string Name { get; set; }
    public string ContactUrl { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int ProviderTypeId { get; set; }
    public int StatusId { get; set; }
    public GetProviderAddress Address { get; set; }

    public record GetProviderAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}