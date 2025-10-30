using Recruit.Shared.Web.ViewModels;
using System.Collections.Generic;

namespace Recruit.Shared.Web.Mappers;

public class ReviewFieldMappingLookupsForPage(
    IReadOnlyList<ReviewFieldIndicatorViewModel> viewModels,
    IDictionary<string, IEnumerable<string>> vacancyMappings)
{
    public IReadOnlyList<ReviewFieldIndicatorViewModel> FieldIdentifiersForPage { get; } = viewModels;

    public IDictionary<string, IEnumerable<string>> VacancyPropertyMappingsLookup { get; } = vacancyMappings;
}