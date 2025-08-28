using System.Linq;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Shared.Web.Domain;

public static class GetAddressesListItemExtensions
{
    public static string ToShortAddress(this GetAddressesListItem item)
    {
        return string.Join(", ", new[]
        {
            item.AddressLine1,
            item.AddressLine2,
        }.Where(x => !string.IsNullOrEmpty(x)));
    }
}