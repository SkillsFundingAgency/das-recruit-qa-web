using Microsoft.AspNetCore.Mvc;

namespace Recruit.Shared.Web.ViewModels;

public class AlertDismissalEditModel
{
    [FromRoute]
    public long Ukprn { get; set; }
    public string AlertType { get; set; }
    public string ReturnUrl { get; set; }
    public string EmployerAccountId { get; set; }
}