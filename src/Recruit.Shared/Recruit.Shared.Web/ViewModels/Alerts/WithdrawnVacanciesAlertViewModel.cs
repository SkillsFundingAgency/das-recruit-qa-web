using System.Collections.Generic;

namespace Recruit.Shared.Web.ViewModels.Alerts;

public class WithdrawnVacanciesAlertViewModel
{
    public List<string> ClosedVacancies { get; set; }
        
    public int ClosedVacanciesCount => ClosedVacancies.Count;

    public bool HasMultipleClosedVacancies => ClosedVacanciesCount > 1;
    public long Ukprn { get; set; }
}