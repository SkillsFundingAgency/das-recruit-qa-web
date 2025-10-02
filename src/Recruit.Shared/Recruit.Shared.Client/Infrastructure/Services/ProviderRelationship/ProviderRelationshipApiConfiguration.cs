using SFA.DAS.Http.Configuration;

namespace Recruit.Vacancies.Client.Infrastructure.Services.ProviderRelationship;

public class ProviderRelationshipApiConfiguration : IManagedIdentityClientConfiguration
{
    public string ApiBaseUrl { get; set; }
    public string IdentifierUri { get; set; }
}