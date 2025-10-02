using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Humanizer;
using Recruit.Shared.Web.Extensions;

namespace Recruit.Shared.Web.Helpers;

public static class VacancyFilterHeadingHelper
{
    public static string GetFilterHeading(string vacancyTerm, int totalVacancies, FilteringOptions filteringOption, string searchTerm, UserType? userType = null)
    {
        var vacancyWord = vacancyTerm.ToQuantity(totalVacancies, ShowQuantityAs.None);

        var words = new List<string>();

        words.Add(totalVacancies.ToString());

        switch(filteringOption)
        {
            case FilteringOptions.All:
                words.Add(vacancyWord);
                break;
            case FilteringOptions.Closed:
            case FilteringOptions.Draft:
            case FilteringOptions.Live:
            case FilteringOptions.Referred:                
                words.Add(filteringOption.GetDisplayName(userType).ToLowerInvariant());
                words.Add(vacancyWord);
                break;
            default:
                words.Add(vacancyWord);
                words.Add(filteringOption.GetDisplayName(userType).ToLowerInvariant());
                break;
        }
            
        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            words.Add($"with '{searchTerm}'");
        }

        return string.Join(" ", words);
    }
}