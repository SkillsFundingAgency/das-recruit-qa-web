using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Models;

namespace Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;

public static class TrainingProviderMapper
{
    public static Domain.Entities.TrainingProvider MapFromApiProvider(Provider provider)
    {
        return new Domain.Entities.TrainingProvider
        {
            Ukprn = provider.Ukprn,
            Name = provider.Name,
            Address = GetAddress(provider.Address)
        };
    }
        
    private static Address GetAddress(ProviderAddress address)
    {
        return new Address
        {
            AddressLine1 = address.Address1,
            AddressLine2 = address.Address2,
            AddressLine3 = address.Address3,
            AddressLine4 = address.Address4,
            Postcode = address.Postcode
        };
    }
}