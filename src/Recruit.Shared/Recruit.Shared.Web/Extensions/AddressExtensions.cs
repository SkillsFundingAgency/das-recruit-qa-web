using System.Collections.Generic;
using System.Linq;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Extensions;

namespace Recruit.Shared.Web.Extensions;

public static class AddressExtensions
{
    public static string ToAddressString(this IAddress address)
    {
        var addressArray = new[] 
        {
            address.AddressLine1, 
            address.AddressLine2, 
            address.AddressLine3, 
            address.AddressLine4, 
            address.Postcode 
        };
        return string
            .Join(", ", addressArray.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()));
    }

    public static string GetLastNonEmptyField(this Address address)
    {
        return new[]
        {
            address.AddressLine4,
            address.AddressLine3,
            address.AddressLine2,
            address.AddressLine1,
        }.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
    }

    public static string ToSingleLineFullAddress(this Address address)
    {
        string[] addressArray = [address.AddressLine1, address.AddressLine2, address.AddressLine3, address.AddressLine4, address.Postcode];
        return string.Join(", ", addressArray.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()));
    }
    
    public static string ToSingleLineAbridgedAddress(this Address address)
    {
        return $"{address.GetLastNonEmptyField()} ({address.Postcode})";
    }
    
    public static string ToSingleLineAnonymousAddress(this Address address)
    {
        return $"{address.GetLastNonEmptyField()} ({address.PostcodeAsOutcode()})";
    }
        
    public static IEnumerable<Address> OrderByCity(this IEnumerable<Address> addresses)
    {
        return addresses.OrderBy(x => x.GetLastNonEmptyField());
    }

    public static IEnumerable<string> GetPopulatedAddressLines(this Address address)
    {
        return new[]
        {
            address.AddressLine1,
            address.AddressLine2,
            address.AddressLine3,
            address.AddressLine4,
            address.Postcode
        }.Where(x => !string.IsNullOrEmpty(x?.Trim()));
    }
}