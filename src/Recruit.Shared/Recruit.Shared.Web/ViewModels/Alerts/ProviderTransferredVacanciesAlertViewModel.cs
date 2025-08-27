using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Recruit.Shared.Web.Extensions;

namespace Recruit.Shared.Web.ViewModels.Alerts;

public class ProviderTransferredVacanciesAlertViewModel
{
    public List<string> LegalEntityNames { get; internal set; }

    public string LegalEntityNamesCaption => LegalEntityNames.Humanize().RemoveOxfordComma();

    public bool HasTransfersToMultipleEmployers => LegalEntityNames.Count() > 1;
    public long Ukprn { get; set; }
}