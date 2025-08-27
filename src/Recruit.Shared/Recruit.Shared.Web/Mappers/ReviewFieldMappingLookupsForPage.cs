using Recruit.Shared.Web.ViewModels;
using System.Collections.Generic;

namespace Recruit.Shared.Web.Mappers;

public class ReviewFieldMappingLookupsForPage
{
    public ReviewFieldMappingLookupsForPage(IReadOnlyList<ReviewFieldIndicatorViewModel> viewModels, IDictionary<string, IEnumerable<string>> vacancyMappings)
    {
        FieldIdentifiersForPage = viewModels;
        VacancyPropertyMappingsLookup = vacancyMappings;
    }

    public IReadOnlyList<ReviewFieldIndicatorViewModel> FieldIdentifiersForPage { get; }

    public IDictionary<string, IEnumerable<string>> VacancyPropertyMappingsLookup { get; }
}